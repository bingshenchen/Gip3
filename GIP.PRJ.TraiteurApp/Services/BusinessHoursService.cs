/*using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BusinessHoursService : IBusinessHoursService
{
    private readonly TraiteurAppDbContext _context;

    public BusinessHoursService(TraiteurAppDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessHours> GetBusinessHours()
    {
        var businessHours = await _context.BusinessHours.Include(b => b.Holidays).FirstOrDefaultAsync();

        if (businessHours == null)
        {
            var businessHours1 = new BusinessHours().ForceClosed;
            businessHours = new BusinessHours
            {
                Id = 1,
                DayOfWeek = "Van Dinsdag Tot Zondag",
                ClosingDays = "Maandag",
                OpeningTime = TimeSpan.Parse("09:00"),
                ClosingTime = TimeSpan.Parse("17:00"),
                IsClosed = businessHours1
            };

            _context.BusinessHours.Add(businessHours);
            await _context.SaveChangesAsync();
        }

        return businessHours;
    }

    public async Task SetOpeningTime(TimeSpan openingTime)
    {
        var businessHours = await GetBusinessHours();
        businessHours.OpeningTime = openingTime;
        await _context.SaveChangesAsync();
    }

    public async Task SetClosingTime(TimeSpan closingTime)
    {
        var businessHours = await GetBusinessHours();
        businessHours.ClosingTime = closingTime;
        await _context.SaveChangesAsync();
    }

    public async Task AddHoliday(DateTime holidayDate)
    {
        var businessHours = await GetBusinessHours();
        businessHours.Holidays.Add(new Holiday { Date = holidayDate });
        await _context.SaveChangesAsync();
    }

    public async Task RemoveHoliday(DateTime holidayDate)
    {
        var businessHours = await GetBusinessHours();
        var holiday = businessHours.Holidays.FirstOrDefault(h => h.Date == holidayDate);
        if (holiday != null)
        {
            businessHours.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetIsClosed(bool isClosed)
    {
        var businessHours = await GetBusinessHours();
        businessHours.IsClosed = businessHours.ForceClosed;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBusinessHours(BusinessHours businessHours)
    {
        _context.Entry(businessHours).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}*/