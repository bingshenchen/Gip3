using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IOrderService _orderService;
        private readonly ICookService _cookService;

        public TimeSlotService(IOrderService orderService, ICookService cookService)
        {
            _orderService = orderService;
            _cookService = cookService;
        }

        /// <summary>
        /// Checks a timeslot to the requested margin (in hours). True if still ok, false otherwise
        /// </summary>
        /// <param name="reservedTime">timeslot in string format HH:mm</param>
        /// <param name="margin">requested margin (time left) in hours</param>
        /// <returns></returns>
        public bool CheckTimeSlot(string reservedTime, int margin)
        {
            // get hour en minutes to create TimeOnly object
            int reservedHour = 0;
            int.TryParse(reservedTime.Substring(0, 2), out reservedHour);
            int reservedMinutes = 0;
            int.TryParse(reservedTime.Substring(3, 2), out reservedMinutes);

            // compare 
            return !(new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute).AddHours(margin) > new TimeOnly(reservedHour, reservedMinutes));
        }

        public async Task<bool> OrderIsLocked(int orderid)
        {
            Order order = await _orderService.GetOrderByIdAsync(orderid);
            if (order == null)
            {
                return true;
            }
            if (!CheckTimeSlot(order.TimeSlot, 1) || (order.OrderedOn < DateTime.Today))
            {
                return true;
            }
            return false;
        }

        public async Task<Dictionary<string, string>> GetTimeSlotDictionary()
        {
            /// get list of today's orders, group by the selected timeslots and select only those 
            /// timeslots that are already selected #cooks times
            /// Key property after GroupBy = property that represents the grouped by value.
            /// 
            int nrOfConcurrentSlots = (await _cookService.GetAllCooksAsync()).Count();

            var reservedSlots = (await _orderService.GetOrdersFromDateAsync(DateTime.Today))
                .GroupBy(o => o.TimeSlot).Select(o => new { o.Key, count = o.Count() })
                .Where(o => o.count >= nrOfConcurrentSlots).Select(o => o.Key);

            /// Timeslots only possible from 11 o'clock
            int startHour = DateTime.Now.Hour + 1;
            if (startHour < 11)
            {
                startHour = 11;
            }

            int startMinute = ((DateTime.Now.Minute / 15) + 1) * 15;
            if (startMinute == 60)
            {
                startMinute = 0;
                startHour++;
            }

            /// create and add timeslots until 23 o'clock (last possible slot = 22:45)
            Dictionary<string, string> timeslots = new Dictionary<string, string>();

            for (int i = startHour; i < 23; i++)
            {
                for (int j = startMinute; j <= 45; j += 15)
                {
                    string timeslot = i.ToString() + ":" + j.ToString("0#");
                    if (!reservedSlots.Contains(timeslot))
                    {
                        timeslots.Add(timeslot, timeslot);
                    }
                }
                startMinute = 0;
            }
            return timeslots;
        }
    }
}
