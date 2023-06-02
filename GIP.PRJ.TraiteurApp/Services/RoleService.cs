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

        public async Task CreateUserRole(UserViewModel uv)
        {
            try
            {
                var user = new IdentityUser { Email = uv.Email, UserName = uv.RoleName };
                IdentityResult result = await _userManager.CreateAsync(user, "Password132..");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Cook");
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);

                }
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsersIdentity()
        {
            try
            {
                return await _userManager.Users.ToListAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            try
            {
                return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);    
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public async Task GetUsersRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            if (roles != null)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(UserViewModel uv)
        {
            try
            {
                var user = new IdentityUser {Email = uv.Email, UserName = uv.RoleName};
                IdentityResult result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);

                }
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}
        
