using GIP.PRJ.TraiteurApp.Data;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesService _rolesService;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRolesService rolesService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _rolesService = rolesService;
        }

        // GET: CreateRolesViewModels
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<CreateRolesViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new CreateRolesViewModel();

                thisViewModel.IdentityUserId = user.Id;
                thisViewModel.Name = user.UserName;
                thisViewModel.Email = user.Email;
                //thisViewModel.RoleNames = await _rolesService.GetUsersRoles();
            }
            return View(userRoles);
        }
        
    } 
}
