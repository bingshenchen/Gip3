using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class Cook
    {
        public int Id { get; set; }
        public int YearsOfExperience { get; set; }
        public string ChefName { get; set; }
        public string IdentityUserId { get; set; }
        public DateTime HolidayStartTime { get; set; }
        public DateTime HolidayEndTime { get; set; }
        public DateTime SickStartTime { get; set; }
        public DateTime SickEndTime { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsSick { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        
    }
}
