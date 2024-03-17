using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using Microsoft.AspNetCore.Authorization;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using GIP.PRJ.TraiteurApp.Services;
using Kendo.Mvc.Extensions;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator, Customer")]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMenuItemCategoryService _menuItemCategoryService;

        public MenuItemsController(IMenuItemService menuItemService, IOrderDetailService orderDetailService,
            IMenuItemCategoryService menuItemCategoryService)
        {
            _menuItemService = menuItemService;
            _orderDetailService = orderDetailService;
            _menuItemCategoryService = menuItemCategoryService;
        }

        // GET: MenuItems
        public IActionResult Index()
        {
              return View();
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.MenuItemCategories = new SelectList(await _menuItemCategoryService.GetAllMenuItemCategoriesAsync(),
                    "Id", "Name");
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,MenuItemCategoryId")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                await _menuItemService.CreateMenuItemAsync(menuItem);
                ViewBag.MenuItemCategories = new SelectList(await _menuItemCategoryService.GetAllMenuItemCategoriesAsync(), 
                    "Id", "Name");
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewBag.MenuItemCategories = new SelectList(await _menuItemCategoryService.GetAllMenuItemCategoriesAsync(),
                    "Id", "Name");
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,MenuItemCategoryId")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _menuItemService.UpdateMenuItemAsync(menuItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await MenuItemExists(menuItem.Id))
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
            ViewBag.MenuItemCategories = new SelectList(await _menuItemCategoryService.GetAllMenuItemCategoriesAsync(),
                    "Id", "Name");
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noDelete = _orderDetailService.AnyMenuItemAsync(id.Value);
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id.Value);

            if ((menuItem) == null)
            {
                return NotFound();
            }

            /// Any: function that will return true if an orderdetail with the selected menuitem is found
            if (await noDelete)
            {
                return View("NoDelete", menuItem);
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MenuItemExists(int id)
        {
            return (await _menuItemService.GetMenuItemByIdAsync(id)) != null;
        }

        #region Kendo 
        public async Task<IActionResult> GetMenuItems([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request)
        {
            try
            {
                // data if Administrator
                var menuItems = await _menuItemService.GetAllMenuItemsAsync();
                return Json(menuItems.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
        #endregion
    }
}
