using GIP.PRJ.TraiteurApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIP.PRJ.TraiteurApp.ViewModels.Admin
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
