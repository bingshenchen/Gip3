using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using GIP.PRJ.TraiteurApp.Services;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using GIP.PRJ.TraiteurApp.ViewModels.Admin;

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
            /*var traiteurAppDbContext = _context.CreateRolesViewModel.Include(c => c.IdentityUser);
            return View(await traiteurAppDbContext.ToListAsync());*/
            var role = (from r in _context.Roles where r.Name.Contains("Administrator") select r).FirstOrDefaultAsync();
            var admins = await _context.Users.ToListAsync();

            var adminVM = admins.Select(user => new CreateRolesViewModel
            {
                Email = user.Email,
                RoleName = "Administrator"

            }).ToList();

            var role2 = (from r in _context.Roles where r.Name.Contains("Cook") select r).FirstOrDefaultAsync();
            var cooks = await _context.Users.ToListAsync();

            var cookVM = cooks.Select(cook => new CreateRolesViewModel
            {
                Email = cook.Email,
                RoleName = "Cook"

            }).ToList();

            var model = new GroupedViewModel { Admins = adminVM, Users = cookVM };
            return View(model);
        }

        // GET: CreateRolesViewModels/Details/5
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
            CreateRolesViewModel vm = new CreateRolesViewModel();
            vm.Roles = _context.Roles.ToList();
            return View(vm);
        }

        // POST: CreateRolesViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,RoleId")] CreateRolesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newUser = new IdentityUser(vm.Email);
                IdentityResult identityResult = await _userManager.CreateAsync(newUser, "Password");
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, vm.RoleName);
                }

                return RedirectToAction(nameof(Index));
            }
            vm.Roles = _context.Roles.ToList();
            return View(vm);
        }

        // GET: CreateRolesViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CreateRolesViewModel == null)
            {
                return NotFound();
            }

            var createRolesViewModel = await _context.CreateRolesViewModel.FindAsync(id);
            if (createRolesViewModel == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(createRolesViewModel);
        }

        // POST: CreateRolesViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,IdentityUserId,Roles")] CreateRolesViewModel createRolesViewModel)
        {
            if (id != createRolesViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(createRolesViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreateRolesViewModelExists(createRolesViewModel.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(createRolesViewModel);
        }

        // GET: CreateRolesViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: CreateRolesViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreateRolesViewModel == null)
            {
                return Problem("Entity set 'TraiteurAppDbContext.CreateRolesViewModel'  is null.");
            }
            var createRolesViewModel = await _context.CreateRolesViewModel.FindAsync(id);
            if (createRolesViewModel != null)
            {
                _context.CreateRolesViewModel.Remove(createRolesViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreateRolesViewModelExists(int id)
        {
          return _context.CreateRolesViewModel.Any(e => e.Id == id);
        }
    }
}
