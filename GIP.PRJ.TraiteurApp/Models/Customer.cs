using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Models
{
    public enum CustomerRating
    { 
        A,
        B
    }
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        [Display(Name = "")]
        public CustomerRating Rating { get; set; }

        [Display(Name = "")]
        public string Info { get; set; }

        [Display(Name = "")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string EmailAddress { get; set; }

        [Display(Name = "")]
        public string IdentityUserId { get; set; }

        [Display(Name = "")]
        public string CompanyName { get; set; }

        [Display(Name = "")]
        [RegularExpression(@"BE\d{10}", ErrorMessage = "Ongeldig BTW-nummer.")]
        public string VATNumber { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "Adres is verplicht.")]
        public string Address { get; set; }

        [Display(Name = "")]
        public bool ConfirmationSent { get; set; }

        [Display(Name = "")]
        public virtual IdentityUser IdentityUser { get; set; }

        [Display(Name = "")]
        public virtual ICollection<Order> Orders { get; set; }

    }
}