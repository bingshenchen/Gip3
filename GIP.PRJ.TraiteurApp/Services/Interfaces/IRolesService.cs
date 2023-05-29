using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task<List<IdentityRole>> GetUsersRoles();
        Task<IEnumerable<CreateRolesViewModel>> GetUsersIdentity();
        Task UpdateUserAsync(IdentityUser identityuser);
        Task DeleteUserAsync(int id);
    }
}
