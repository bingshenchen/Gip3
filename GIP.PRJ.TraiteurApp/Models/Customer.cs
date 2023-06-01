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

        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Name { get; set; }

        public CustomerRating Rating { get; set; }
        public string Info { get; set; }

        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string EmailAddress { get; set; }

        public string IdentityUserId { get; set; }
        public string CompanyName { get; set; }

        [RegularExpression(@"BE\d{10}", ErrorMessage = "Ongeldig BTW-nummer.")]
        public string VATNumber { get; set; }

        [Required(ErrorMessage = "Adres is verplicht.")]
        public string Address { get; set; }

        public bool ConfirmationSent { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}