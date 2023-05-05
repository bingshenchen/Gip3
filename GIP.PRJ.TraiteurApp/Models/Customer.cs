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
        public string Name { get; set; }
        public CustomerRating Rating { get; set; }
        public string Info { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityUserId { get; set; }
        public string CompanyName { get; set; }
        public string VATNumber { get; set; }
        public string Address { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}