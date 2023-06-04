using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TraiteurAppDbContext _context;

        public AdminService(TraiteurAppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<List<UserViewModel>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRoleViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var r = await _userManager.GetRolesAsync(user);
                userRoleViewModels.Add(new UserViewModel { RoleName = string.Join(", ", r), Email = user.Email, UserId = user.Id });
            }

            return userRoleViewModels;
        }

        public async Task<UserViewModel> GetUserViewModelByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    RoleName = string.Join(", ", roles)
                };
            }
            return null;
        }

        public async Task<DataSourceResult> GetAdminsAsync(DataSourceRequest request)
        {
            var adminList = _context.Users.Select(user => new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                RoleName = _context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_context.Roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => role.Name)
                    .FirstOrDefault()
            });

            return await adminList.ToDataSourceResultAsync(request);
        }

        public async Task CreateUserRole(UserViewModel uv)
        {
            var user = new IdentityUser { Email = uv.Email, UserName = uv.Email };
            var result = await _userManager.CreateAsync(user, "Password132..");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roleExists = await _roleManager.RoleExistsAsync(uv.RoleName);
            if (!roleExists)
            {
                var role = new IdentityRole { Name = uv.RoleName };
                result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            result = await _userManager.AddToRoleAsync(user, uv.RoleName);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task UpdateUserAsync(UserViewModel uv)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == uv.UserId);
            if (user != null)
            {
                user.Email = uv.Email;
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                var result = await _userManager.AddToRoleAsync(user, uv.RoleName);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

    }
}