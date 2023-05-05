using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using NuGet.Common;
using Microsoft.Identity.Client;
using System.Net.Mail;
using System.Net;
using GIP.PRJ.TraiteurApp.Services;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Customer")] 
    public class CustomerOrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMailService _mailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITimeSlotService _timeSlotService;

        public CustomerOrdersController(IOrderService orderService, ICustomerService customerService, IOrderDetailService orderDetailService, 
            IMailService mailService, UserManager<IdentityUser> userManager, ITimeSlotService timeSlotService)
        {
            _orderService = orderService;
            _customerService = customerService; 
            _orderDetailService = orderDetailService;
            _mailService = mailService;
            _userManager = userManager;
            _timeSlotService = timeSlotService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            /// show the customer name in the view by using a ViewBag 
            /// 
            Customer customer = await _customerService.GetCustomerByIdentityAsync(_userManager.GetUserId(User));
            ViewBag.CustomerName = customer?.Name ?? string.Empty;
            ViewBag.CustomerId = customer?.Id ?? 0;

            return View();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.IsLocked = await _timeSlotService.OrderIsLocked(order.Id); 
            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            Customer customer = await _customerService.GetCustomerByIdentityAsync(_userManager?.GetUserId(User));
            Order order = new Order{ OrderedOn = DateTime.Now };
            if (customer == null)
            {
                return NotFound();
            }
            order.CustomerId = customer.Id;
            
            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");

            /// set the OrderedOn date = DateTime.Now
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderedOn,TimeSlot,CustomerId")] Order order)
        {
            Customer customer = await _customerService.GetCustomerByIdentityAsync(_userManager?.GetUserId(User));
            if (customer.Id != order.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(order);
                return RedirectToAction("Create", "OrderDetails", new { id = order.Id });
            }

            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            Customer customer = await _customerService.GetCustomerByIdentityAsync(_userManager?.GetUserId(User));

            if (customer.Id != order.CustomerId)
            {
                return NotFound();
            }

            if (await _timeSlotService.OrderIsLocked(order.Id))
            {
                return RedirectToAction(nameof(Details), order);
            }

            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderedOn,TimeSlot,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            Customer customer = await _customerService.GetCustomerByIdentityAsync(_userManager?.GetUserId(User));

            if (customer.Id != order.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateOrderAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderExists(order.Id))
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

            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");
            return View(order);
        }

        private async Task<bool> OrderExists(int id)
        {
            return (await _orderService.GetOrderByIdAsync(id)) != null;
        }

        public async Task<IActionResult> GetOrders([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdentityAsync(_userManager.GetUserId(User));
                if (customer == null)
                {
                    return Json(string.Empty);
                }
                var orders = await _orderService.GetOrdersByCustomerAsync(customer.Id);

                return Json(orders.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
    }
}
