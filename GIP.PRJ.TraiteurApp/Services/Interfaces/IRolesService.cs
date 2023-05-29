using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task GetUsersRoles();
        Task<IEnumerable<UserViewModel>> GetAllUsersIdentity();
        Task<UserViewModel> GetUserByIdAsync(string id);
        Task<UserViewModel> GetUserbyId(int id);
        Task CreateUserRole(UserViewModel createRoles);
        Task UpdateUserAsync(UserViewModel createRoles);
        Task DeleteUserAsync(int id);
    }
}
