using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Kendo.Mvc.UI;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IMenuItemService _menuItemService;
        private readonly ITimeSlotService _timeSlotService;

        public OrderDetailsController(IOrderDetailService orderDetailService, IOrderService orderService, 
            IMenuItemService menuItemService, ITimeSlotService timeSlotService)
        {
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _menuItemService = menuItemService;
            _timeSlotService = timeSlotService;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index(int id)
        {
            /// show details of orderid (= id)
            /// 
            ViewBag.OrderId = id;
            ViewBag.IsLocked = false;
            Order order = await _orderService.GetOrderByIdAsync(id);
            if (order != null)
            {
                if (order.IsPaid)
                {
                    return RedirectToAction("Invoice", "Orders", new { id = id });
                } 
                ViewBag.IsLocked = await _timeSlotService.OrderIsLocked(id);
                
            }
            var orderDetails = _orderDetailService.GetOrderDetailsByOrderAsync(id);
            return View(await orderDetails);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id.Value);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        /// <summary>
        /// Creates a new orderdetail for the specified order (= id parameter)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int id)
        {
            if (await _timeSlotService.OrderIsLocked(id))
            {
                return RedirectToAction(nameof(Index), new { id });
            }
            ViewBag.Message = TempData["Message"];
            ViewData["MenuItemId"] = new SelectList(await _menuItemService.GetAllMenuItemsAsync(), "Id", "Name");

            
            {
                ViewBag.IsLocked = await _timeSlotService.OrderIsLocked(id);
            }
            /// set OrderId (to link OrderDetail to the Order) + set default value for Quantity to 1
            return View(new OrderDetail { OrderId = id, Quantity = 1});
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,MenuItemId,Quantity,Price")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                /// get item price
                /// 
                Models.MenuItem menuItem = await _menuItemService.GetMenuItemByIdAsync(orderDetail.MenuItemId);
                orderDetail.Price = menuItem?.Price ?? 0;

                await _orderDetailService.CreateOrderDetailAsync(orderDetail);
                TempData["Message"] = orderDetail.Quantity + " x " + menuItem?.Name + " werd aan de bestelling toegevoegd.";
                return RedirectToAction("Create", "OrderDetails", new { id = orderDetail.OrderId});
            }

            ViewData["MenuItemId"] = new SelectList(await _menuItemService.GetAllMenuItemsAsync(), "Id", "Name", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id.Value);
            if (orderDetail == null)
            {
                return NotFound();
            }
            
            if (await _timeSlotService.OrderIsLocked(orderDetail.OrderId))
            {
                return RedirectToAction(nameof(Index), new { id });
            }

            ViewData["MenuItemId"] = new SelectList(await _menuItemService.GetAllMenuItemsAsync(), "Id", "Id", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,MenuItemId,Quantity,Price")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderDetailExists(orderDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = orderDetail.OrderId});
            }
            ViewData["MenuItemId"] = new SelectList(await _menuItemService.GetAllMenuItemsAsync(), "Id", "Id", orderDetail.MenuItemId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id.Value);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            await _orderDetailService.DeleteOrderDetailAsync(id);
            return RedirectToAction(nameof(Index), new { id = orderDetail.OrderId});
        }

        private async Task<bool> OrderDetailExists(int id)
        {
            return await _orderDetailService.GetOrderDetailByIdAsync(id) != null;
        }
    }
}
