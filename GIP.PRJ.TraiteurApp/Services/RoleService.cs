using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.ViewModels;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class RoleService : IRolesService
    {
        //Many to many relation between these roles
        //A user can be a member of any given roles 
        //A role can be assigned to many users
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TraiteurAppDbContext _context;

        public RoleService(TraiteurAppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task CreateUserRole(UserViewModel createRoles)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserViewModel>> GetAllUsersIdentity()
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> GetUserbyId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task GetUsersRoles()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(UserViewModel createRoles)
        {
            throw new NotImplementedException();
        }
    }
}
