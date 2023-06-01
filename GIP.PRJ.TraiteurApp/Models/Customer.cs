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
        [Display(Name = "")]
        public int Id { get; set; }

        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        [Display(Name = "Beoordeling")]
        public CustomerRating Rating { get; set; }

        [Display(Name = "Informatie")]
        public string Info { get; set; }

        [Display(Name = "E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string EmailAddress { get; set; }

        [Display(Name = "Gebruikers-ID")]
        public string IdentityUserId { get; set; }

        [Display(Name = "Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [Display(Name = "BTW-nummer")]
        [RegularExpression(@"BE\d{10}", ErrorMessage = "Ongeldig BTW-nummer.")]
        public string VATNumber { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Adres is verplicht.")]
        public string Address { get; set; }

        [Display(Name = "Bevestiging verzonden")]
        public bool ConfirmationSent { get; set; }

        [Display(Name = "Gebruiker")]
        public virtual IdentityUser IdentityUser { get; set; }

        [Display(Name = "Bestellingen")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}