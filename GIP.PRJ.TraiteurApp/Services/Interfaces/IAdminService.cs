using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Kendo.Mvc.UI;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetAllUsersWithRoles();
        Task<UserViewModel> GetUserViewModelByIdAsync(string userId);
        Task<DataSourceResult> GetAdminsAsync(DataSourceRequest request);
        Task<IdentityUser> GetUserByIdAsync(string userId);
        Task CreateUserRole(UserViewModel uv);
        Task UpdateUserAsync(UserViewModel uv);
        Task DeleteUserAsync(string id);
    }
}