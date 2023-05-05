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
    }
}
