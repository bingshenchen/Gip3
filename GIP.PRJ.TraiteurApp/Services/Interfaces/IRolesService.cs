using GIP.PRJ.TraiteurApp.Migrations;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IRolesService
    {
        Task GetUsersRoles();
        Task<IEnumerable<CreateRolesViewModel>> GetAllUsersIdentity();
        Task<CreateRolesViewModel> GetUserByIdAsync(string id);
        Task<CreateRolesViewModel> GetUserbyId(int id);
        Task CreateUserRole(CreateRolesViewModel createRoles);
        Task UpdateUserAsync(CreateRolesViewModel createRoles);
        Task DeleteUserAsync(int id);
    }
}
