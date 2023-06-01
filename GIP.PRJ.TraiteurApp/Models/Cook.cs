using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class Cook
    {
        [Display(Name = "")]
        public int Id { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "Chef Naam is verplicht.")]
        public string ChefName { get; set; }

        [Display(Name = "")] 
        public int YearsOfExperience { get; set; }

        [Display(Name = "")]
        public string IdentityUserId { get; set; }

        [Display(Name = "")]
        public virtual IdentityUser IdentityUser { get; set; }

        [Display(Name = "")]
        public virtual ICollection<Order> Orders { get; set; }

        [Display(Name = "")]
        public DateTime HolidayStartTime { get; set; }

        [Display(Name = "")]
        public bool IsHoliday { get; set; }

        [Display(Name = "")]
        public DateTime SickStartTime { get; set; }

        [Display(Name = "")]
        public bool IsSick { get; set; }

        [Display(Name = "")]
        public DateTime HolidayEndTime { get; set; }

        [Display(Name = "")]
        public DateTime SickEndTime { get; set; }
    }
}
