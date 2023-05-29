using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.ViewModels;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using System.Linq;

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
        //return new List<IEnumerable<CreateRolesViewModel>>(await _userManager.GetUsersInRoleAsync(userId));
    /*    public async Task<IEnumerable<CreateRolesViewModel>> GetUsersIdentity()
        {
            try
            {
                var usersz = await _userManager.Users.ToListAsync();
                var viewModels = usersz.Select(u => new CreateRolesViewModel
                {
                    Id = int.TryParse(u.Id, out int id) ? id : 0,
                    Name = u.UserName,
                    Email = u.Email
                });

                return viewModels;
            }
            catch (Exception ex)
            {
                throw new Exception($"Users data corrupted", ex);
            }
        }
*/
        /*public async Task<List<IdentityRole>> GetUsersRoles()
        {
            try
            {
                var rolesz = _roleManager.Roles;
                return await rolesz.ToListAsync();

            }
            catch (Exception ex)
            {

                throw new Exception($"User Roles Corrupted", ex);
            }
            
            //return new List<IEnumerable<CreateRolesViewModel>>(await _roleManager.GetRoleNameAsync(identityRole));
        }*/

        public async Task GetUsersRoles()
        {
            try
            {
                

               
                /*var rolesz = _roleManager.Roles;
                return await rolesz.ToListAsync();*/

            }
            catch (Exception ex)
            {

                throw new Exception($"User Roles Corrupted", ex);
            }
        }

        public Task<IEnumerable<CreateRolesViewModel>> GetAllUsersIdentity()
        {
            throw null;
        }

        public Task<CreateRolesViewModel> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CreateRolesViewModel> GetUserbyId(int id)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserRole(CreateRolesViewModel createRoles)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(CreateRolesViewModel createRoles)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(int id)
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
