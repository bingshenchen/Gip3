using GIP.PRJ.TraiteurApp.Data;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class RoleService : IRolesService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        GIPPRJTraiteurAppContext appContext;
        public RoleService(GIPPRJTraiteurAppContext traiteurAppContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            appContext = traiteurAppContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<CreateRolesViewModel>>GetUsersIdentity(IdentityUser userId)
        {
            //return new List<IEnumerable<CreateRolesViewModel>>(await _userManager.GetUsersInRoleAsync(userId));
            try
            {
                return (IEnumerable<CreateRolesViewModel>)await _userManager.Users.Include(u => u.UserName).ToListAsync();

            }
            catch (Exception ex)
            {
                
                throw new Exception("UsersFailed");
            }

        }

        public async Task<IEnumerable<CreateRolesViewModel>> GetUsersRoles(IdentityRole identityRole)
        {
            throw null;
            //return new List<IEnumerable<CreateRolesViewModel>>(await _roleManager.GetRoleNameAsync(identityRole));
        }
        public Task CreateUserAsync(IdentityUser identityUser)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(IdentityUser identityuser)
        {
            throw new NotImplementedException();
        }



        /*public async Task UserRoles()
        {
            var userRoles = (from user in appContext.CreateRolesViewModel
                             select new
                             {
                                 UserID = user.Id,
                                 UserName = user.Name,
                                 UserEmail = user.Email,
                                 Rolenames = (from userRoles in user.RoleNames 
                                              join role in appContext.CreateRolesViewModel on userRoles.Rol)



                             });

        }*/
    }
}
