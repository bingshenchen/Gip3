﻿using System;
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
            var role = await (from r in _context.Roles where r.Name.Contains("Administrator") select r).FirstOrDefaultAsync();
            var admins = await _context.Users.ToListAsync();

            var adminVM = admins.Select(user => new UserViewModel
            {
                Email = user.Email,
                RoleName = "Administrator"

            }).ToList();

            var role2 = await (from r in _context.Roles where r.Name.Contains("Cook") select r).FirstOrDefaultAsync();
            var cooks = await _context.Users.ToListAsync();

            var cookVM = cooks.Select(cook => new UserViewModel
            {
                Email = cook.Email,
                RoleName = "Cook"

            }).ToList();

            var model = adminVM.Concat(cookVM).ToList();
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityUser user = await _rolesService.GetUserByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("", "Details cannot be found");
            }
            return View(user);
        }

        // GET: CreateRolesViewModels/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            UserViewModel user = new UserViewModel();
            user.Roles = _context.Roles.ToList();
            return View(user);
        }

        //POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,RoleName,Roles")] UserViewModel uv)
        {
            if (ModelState.IsValid)
            {
                /*var newUser = new IdentityUser { Email = uv.Email, UserName = uv.Email};
                IdentityResult identityResult = await _userManager.CreateAsync(newUser, "Password132..");
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Cook");
                }*/
                await _rolesService.CreateUserRole(uv);
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

            var user = await _rolesService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
            /*IdentityUser user = await _rolesService.GetUserByIdAsync(id);
            if (user == null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }*/
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,RoleName,Roles")]UserViewModel uv)
        {
            //IdentityUser user = await _rolesService.GetUserByIdAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    await _rolesService.UpdateUserAsync(uv);

                }
                catch (Exception ex)
                {
                    throw new Exception("", ex);
                }
                /*if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction(nameof(Details));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }*/
            }
            return View(uv);
            /*var edituser = await _userManager.FindByIdAsync(id);
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
            return View(edituser);*/
        }

        // GET: Admin/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _rolesService.GetUserByIdAsync(id);
                if (user != null)
                {
                    return View(user);
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _rolesService.GetUserByIdAsync(id); 
            return RedirectToAction(nameof(Index));
        }

        private bool UserViewModelExists(string id)
        {
          return _context.CreateRolesViewModel.Any(e => e.Id == id);
        }

        public IActionResult GetAdmin([DataSourceRequest] DataSourceRequest request)
        {
            var adminList = _context.Users.Select(user => new UserViewModel
            {
                Email = user.Email,
                RoleName = _context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_context.Roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => role.Name)
                    .FirstOrDefault()
            });

            var result = adminList.ToDataSourceResult(request);
            return Json(result);
        }

        private async Task<bool> UserExists(string id)
        {
            return (await _rolesService.GetUserByIdAsync(id)) != null;
        }
    }
}
