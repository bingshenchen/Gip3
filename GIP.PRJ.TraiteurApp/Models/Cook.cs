using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class Cook
    {
        [Display(Name = "")]
        public int Id { get; set; }

        [Display(Name = "Chef Naam")]
        [Required(ErrorMessage = "Chef Naam is verplicht.")]
        public string ChefName { get; set; }

        [Display(Name = "Jarenlange Ervaring")]
        public int YearsOfExperience { get; set; }

        [Display(Name = "")]
        public string IdentityUserId { get; set; }

        [Display(Name = "")]
        public virtual IdentityUser IdentityUser { get; set; }

        [Display(Name = "Bestellingen")]
        public virtual ICollection<Order> Orders { get; set; }

        [Display(Name = "Vakantie Van")]
        public DateTime HolidayStartTime { get; set; }

        [Display(Name = "Vakantie Tot")]
        public DateTime HolidayEndTime { get; set; }

        [Display(Name = "Ziekte Van")]
        public DateTime SickStartTime { get; set; }

        [Display(Name = "Ziekte Tot")]
        public DateTime SickEndTime { get; set; }

        [Display(Name = "Is Op Vakantie")]
        public bool IsHoliday { get; set; }

        [Display(Name = "Is Ziek")]
        public bool IsSick { get; set; }
    }
}