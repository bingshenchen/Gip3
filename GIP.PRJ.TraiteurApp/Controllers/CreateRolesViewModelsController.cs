using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Data;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class CreateRolesViewModelsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesService _rolesService;
        private readonly GIPPRJTraiteurAppContext _context;

        public CreateRolesViewModelsController(GIPPRJTraiteurAppContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: CreateRolesViewModels
        public async Task<IActionResult> Index()
        {
            var users = await _rolesService.GetUsersIdentity();
            return View(users);
            /* var gIPPRJTraiteurAppContext = _context.CreateRolesViewModel.Include(c => c.IdentityUser);
             return View(await gIPPRJTraiteurAppContext.ToListAsync());*/
        }

        // GET: CreateRolesViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreateRolesViewModel == null)
            {
                return NotFound();
            }

            var createRolesViewModel = await _context.CreateRolesViewModel
                .Include(c => c.IdentityUser)
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
            ViewData["IdentityUserId"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id");
            return View();
        }

        // POST: CreateRolesViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,IdentityUserId,Roles")] CreateRolesViewModel createRolesViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(createRolesViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", createRolesViewModel.IdentityUserId);
            return View(createRolesViewModel);
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
            ViewData["IdentityUserId"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", createRolesViewModel.IdentityUserId);
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
            ViewData["IdentityUserId"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", createRolesViewModel.IdentityUserId);
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
                .Include(c => c.IdentityUser)
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
                return Problem("Entity set 'GIPPRJTraiteurAppContext.CreateRolesViewModel'  is null.");
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
