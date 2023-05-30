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

namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesService _rolesService;
        private readonly TraiteurAppDbContext _context;

        public AdminController(TraiteurAppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRolesService rolesService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _rolesService = rolesService;
        }

        // GET: CreateRolesViewModels
        public async Task<IActionResult> Index()
        {
            var role = (from r in _context.Roles where r.Name.Contains("Administrator") select r).FirstOrDefaultAsync();
            var admins = await _context.Users.ToListAsync();

            var adminVM = admins.Select(user => new UserViewModel
            {
                Email = user.Email,
                RoleName = "Administrator"

            }).ToList();

            var role2 = (from r in _context.Roles where r.Name.Contains("Cook") select r).FirstOrDefaultAsync();
            var cooks = await _context.Users.ToListAsync();

            var cookVM = cooks.Select(cook => new UserViewModel
            {
                Email = cook.Email,
                RoleName = "Cook"

            }).ToList();

            var model = new GroupedViewModel { Admins = adminVM, Users = cookVM };
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreateRolesViewModel == null)
            {
                return NotFound();
            }

            var createRolesViewModel = await _context.CreateRolesViewModel
                .FirstOrDefaultAsync(m => m.Id == id);

            if (createRolesViewModel == null)
            {
                return NotFound();
            }

            return View(createRolesViewModel);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CreateRolesViewModel == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.CreateRolesViewModel.FindAsync(id);
            if (userViewModel == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,RoleName")] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserViewModelExists(userViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreateRolesViewModel == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.CreateRolesViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreateRolesViewModel == null)
            {
                return Problem("Entity set 'TraiteurAppDbContext.CreateRolesViewModel'  is null.");
            }
            var userViewModel = await _context.CreateRolesViewModel.FindAsync(id);
            if (userViewModel != null)
            {
                _context.CreateRolesViewModel.Remove(userViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserViewModelExists(int id)
        {
          return _context.CreateRolesViewModel.Any(e => e.Id == id);
        }
    }
}
