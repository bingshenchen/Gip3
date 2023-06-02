using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.Identity.Client;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class BusinessHoursService : IBusinessHoursService
    {
        public Task CreateClosingDay(BusinessHours businessDays)
        {
            throw new NotImplementedException();
        }

        public Task CreateHolidays(BusinessHours businessHolidays)
        {
            throw new NotImplementedException();
        }

        public Task CreateOpeningTime(BusinessHours businessHours)
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> GetClosingDay()
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetClosingTime()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> GetHolidays()
        {
            throw new NotImplementedException();
        }

        public Task<DateTime> GetOpeningDays()
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetOpeningTime()
        {
            throw new NotImplementedException();
        }
    }
}
