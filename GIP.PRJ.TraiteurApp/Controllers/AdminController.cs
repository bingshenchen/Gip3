using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using GIP.PRJ.TraiteurApp.Services;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IRolesService _rolesService;
        private readonly TraiteurAppDbContext _context;

        public AdminController(TraiteurAppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRolesService rolesService, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _rolesService = rolesService;
            _passwordHasher = passwordHasher;
        }

        // GET: CreateRolesViewModels
        public async Task<IActionResult> Index()
        {
            var role = (from r in _context.Roles where r.Name.Contains("Administrator") select r).FirstOrDefaultAsync();
            var admins = await _userManager.Users.ToListAsync();

            var adminVM = admins.Select(user => new UserViewModel
            {
                Email = user.Email,
                RoleName = "Administrator"

            }).ToList();

            var role2 = (from r in _context.Roles where r.Name.Contains("Cook") select r).FirstOrDefaultAsync();
            var cooks = await _userManager.Users.ToListAsync();

            var cookVM = cooks.Select(cook => new UserViewModel
            {
                Email = cook.Email,
                RoleName = "Cook"

            }).ToList();

            var model = new GroupedViewModel { Admins = adminVM, Users = cookVM };
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detailsusers = await _context.Users.ToListAsync();
            if (detailsusers == null)
            {
                ModelState.AddModelError("", "Details cannot be found");
            }
            return View(detailsusers);
        }

        // GET: CreateRolesViewModels/Create
        public IActionResult Create()
        {
            UserViewModel user = new UserViewModel();
            user.Roles = _context.Roles.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,RoleId")] UserViewModel uv)
        {
            if (ModelState.IsValid)
            {
                var newUser = new IdentityUser(uv.Email);
                IdentityResult identityResult = await _userManager.CreateAsync(newUser, "Password");
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, uv.RoleName);
                }

                return RedirectToAction(nameof(Index));
            }
            uv.Roles = _context.Roles.ToList();
            return View(uv);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            var edituser = await _userManager.FindByIdAsync(id);
            if (edituser != null)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    edituser.Email = email;

                }
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }

                if (!string.IsNullOrEmpty(password))
                {
                    edituser.PasswordHash = _passwordHasher.HashPassword(edituser, password);
                }
                else
                {
                    ModelState.AddModelError("", "Password required");
                }

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult identityResult = await _userManager.UpdateAsync(edituser);
                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return NotFound();
                }

            }
            else
            {
                ModelState.AddModelError("", "User not Found");

            }
            return View(edituser);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleteuser = await _userManager.FindByIdAsync(id);
                if (deleteuser != null)
                {
                    return View(deleteuser);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error", ex);
            }
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var deleteuser = await _userManager.FindByIdAsync(id); 
            if (deleteuser != null) 
            {
                IdentityResult identityResult = await _userManager.DeleteAsync(deleteuser);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(deleteuser);
        }

        private bool UserViewModelExists(int id)
        {
          return _context.CreateRolesViewModel.Any(e => e.Id == id);
        }
    }
}
