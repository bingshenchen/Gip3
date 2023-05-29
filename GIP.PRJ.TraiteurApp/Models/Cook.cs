using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class Cook
    {
        public int Id { get; set; }
        public string ChefName { get; set; }
        public int YearsOfExperience { get; set; }
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public DateTime HolidayStartTime { get; internal set; }
        public bool IsHoliday { get; internal set; }
        public DateTime SickStartTime { get; internal set; }
        public bool IsSick { get; internal set; }
        public DateTime HolidayEndTime { get; internal set; }
        public DateTime SickEndTime { get; internal set; }
    }
}
