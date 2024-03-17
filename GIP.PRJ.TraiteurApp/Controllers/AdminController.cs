using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var model = await _adminService.GetAllUsersWithRoles();
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await _adminService.GetUserViewModelByIdAsync(id);
            if (viewModel == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(new UserViewModel());
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _adminService.CreateUserRole(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _adminService.GetUserViewModelByIdAsync(id);
            return user == null ? RedirectToAction(nameof(Index)) : View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,Email,RoleName")] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _adminService.UpdateUserAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _adminService.GetUserViewModelByIdAsync(id);
            return user == null ? NotFound() : View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _adminService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAdmin([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _adminService.GetAdminsAsync(request);
            return Json(result);
        }
    }
}