using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task GetUsersRoles();
        Task<IEnumerable<IdentityUser>> GetAllUsersIdentity();
        Task<IdentityUser> GetUserByIdAsync(string id);
        Task CreateUserRole(UserViewModel uv);
        Task UpdateUserAsync(UserViewModel uv);
        Task DeleteUserAsync(string id);
    }
}
