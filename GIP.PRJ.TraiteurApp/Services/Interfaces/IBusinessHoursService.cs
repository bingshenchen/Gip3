using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IBusinessHoursService
    {
        //Getters
        Task<TimeSpan>GetOpeningTime();
        Task<TimeSpan>GetClosingTime();
        Task<DateTime> GetOpeningDays();
        Task<DateTime> GetClosingDay();
        Task<DateTime> GetHolidays();

        //admin moet deze functies kunnen intikken op een view
        Task CreateOpeningTime(BusinessHours businessHours);
        Task CreateClosingDay(BusinessHours businessDays);
        Task CreateHolidays(BusinessHours businessHolidays);



    }
}
