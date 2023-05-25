using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task<List<IdentityRole>> GetUsersRoles();
        Task<List<IdentityUser>> GetUsersIdentity();
        Task UpdateUserAsync(IdentityUser identityuser);
        Task DeleteUserAsync(int id);
    }
}
