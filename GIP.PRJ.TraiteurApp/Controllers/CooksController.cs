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

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CooksController : Controller
    {
        private readonly ICookService _cookService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrderService _orderService;

        public CooksController(ICookService cookService, UserManager<IdentityUser> userManager, IOrderService orderService)
        {
            _cookService = cookService;
            _userManager = userManager;
            _orderService = orderService;
        }

        // GET: Cooks
        public async Task<IActionResult> Index()
        {
            var cooks = await _cookService.GetAllCooksAsync();
            return View(cooks);
        }

        // GET: Cooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cook = await _cookService.GetCookByIdAsync(id.Value);
            if (cook == null)
            {
                return NotFound();
            }

            return View(cook);
        }

        // GET: Cooks/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdentityUserId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Cook"), "Id", "UserName");
            return View();
        }

        // POST: Cooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChefName,YearsOfExperience,IdentityUserId")] Cook cook)
        {
            if (ModelState.IsValid)
            {
                await _cookService.CreateCookAsync(cook);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Cook"), "Id", "UserName");
            return View(cook);
        }

        // GET: Cooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cook = await _cookService.GetCookByIdAsync(id.Value);
            if (cook == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Cook"), "Id", "UserName");
            return View(cook);
        }

        // POST: Cooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChefName,YearsOfExperience,IdentityUserId")] Cook cook)
        {
            if (id != cook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _cookService.UpdateCookAsync(cook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CookExists(cook.Id))
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
            ViewData["IdentityUserId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Cook"), "Id", "UserName");
            return View(cook);
        }

        // GET: Cooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cook = await _cookService.GetCookByIdAsync(id.Value);
            if (cook == null)
            {
                return NotFound();
            }
            if ((await _orderService.OrderCountByCook(id.Value)) > 0)
            {
                return View("NoDelete", cook);
            }

            return View(cook);
        }

        // POST: Cooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cookService.DeleteCookAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CookExists(int id)
        {
          return await _cookService.GetCookByIdAsync(id) != null;
        }

        #region Kendo 
        public async Task<IActionResult> GetCooks([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                // data if Administrator
                var cooks = await _cookService.GetAllCooksAsync();
                return Json(cooks.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
        #endregion
    }
}
