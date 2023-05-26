using GIP.PRJ.TraiteurApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class CreateRolesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string IdentityUserId { get; set; }
        public string Roles { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual IdentityRole IdentityRole { get; set; }
    }
}
