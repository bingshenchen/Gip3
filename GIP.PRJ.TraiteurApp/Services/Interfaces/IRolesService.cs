using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<CreateRolesViewModel>> GetUsersRoles(IdentityRole roleId);
        Task<IEnumerable<CreateRolesViewModel>> GetUsersIdentity(IdentityUser userId);
        Task CreateUserAsync(IdentityUser identityUser);
        Task UpdateUserAsync(IdentityUser identityuser);
        Task DeleteUserAsync(int id);
    }
}
