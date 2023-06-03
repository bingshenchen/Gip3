using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService; 
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(IAdminService adminService, UserManager<IdentityUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
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

        //POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,RoleName,Roles")] UserViewModel model)
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
            var user = await _adminService.GetUserByIdAsync(id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var viewModel = new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    RoleName = string.Join(", ", roles)
                };
                return View(viewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Email,RoleName,Roles")] UserViewModel model)
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
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                RoleName = string.Join(", ", roles)
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
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