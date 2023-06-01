using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class Cook
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Chef Naam is verplicht.")]
        public string ChefName { get; set; }
        public int YearsOfExperience { get; set; }
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public DateTime HolidayStartTime { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime SickStartTime { get; set; }
        public bool IsSick { get; set; }
        public DateTime HolidayEndTime { get; set; } 
        public DateTime SickEndTime { get; set; }
    }
}
