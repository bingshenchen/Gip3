
using Microsoft.AspNetCore.Mvc;
using GIP.PRJ.TraiteurApp.Models;
using NuGet.Common;
using Microsoft.Identity.Client;
using System.Net.Mail;
using System.Net;
using GIP.PRJ.TraiteurApp.Services;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Kendo.Mvc.UI;
using GIP.PRJ.TraiteurApp.Migrations;
using static NuGet.Packaging.PackagingConstants;
using Kendo.Mvc.Extensions;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator, Cook")]
    public class CookOrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly ICookService _cookService;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IOrderDetailService _orderDetailService;
        //private readonly IMailService _mailService;

        public CookOrdersController(IOrderService orderService, ICustomerService customerService,
            ICookService cookService, UserManager<IdentityUser> userManager)
            //IOrderDetailService orderDetailService, IMailService mailService)
        {
            _cookService = cookService;
            _orderService = orderService;
            _customerService = customerService; 
            _userManager = userManager;
            //_orderDetailService = orderDetailService;
            //_mailService = mailService;
        }

        public async Task<IActionResult> ToAssignIndex()
        {
            var cook = await _cookService.GetCookByIdentityAsync(_userManager.GetUserId(User));
            if (cook == null)
            {
                return NotFound();
            }

            ViewBag.ChefName = cook?.ChefName ?? string.Empty;
            ViewBag.CookId = cook?.Id ?? 0;

            return View();  
        }

        public async Task<IActionResult> AssignedIndex()
        {
            var cook = await _cookService.GetCookByIdentityAsync(_userManager.GetUserId(User));
            if (cook == null)
            {
                return NotFound();
            }
            
            /// show the customer name in the view by using a ViewBag 
            /// 
            ViewBag.ChefName = cook?.ChefName ?? string.Empty;
            ViewBag.CookId = cook?.Id ?? 0;

            return View();
        }

        public async Task<IActionResult> Assign(int id)
        {
            var orderTask = _orderService.GetOrderByIdAsync(id);
            var cook = await _cookService.GetCookByIdentityAsync(_userManager.GetUserId(User));
            if (cook == null)
            {
                return NotFound();
            }

            var order = await orderTask;
            order.CookId = cook.Id;
            order.Status = OrderStatus.Assigned;
            await _orderService.UpdateOrderAsync(order);

            return RedirectToAction(nameof(ToAssignIndex));
        }

        public async Task<IActionResult> Start(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            order.Status = OrderStatus.Started;
            await _orderService.UpdateOrderAsync(order);

            return RedirectToAction(nameof(AssignedIndex));
        }

        public async Task<IActionResult> Finish(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            order.Status = OrderStatus.Finished;
            await _orderService.UpdateOrderAsync(order);

            return RedirectToAction(nameof(AssignedIndex));
        }

        #region Kendo 
        public async Task<IActionResult> GetAssignedOrders([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                // data if Administrator
                var orders = await _orderService.GetOrdersByCookAsync(id);
                return Json(orders.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }

        public async Task<IActionResult> GetOrdersToAssign([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
               

                // get unassigned orders
                var orders = (await _orderService.GetOrdersByCookAsync(null));

                // get first timeslot to assign cooks to
                var firstTimeSlot = orders.OrderBy(o => o.TimeSlot).FirstOrDefault();

                if (firstTimeSlot != null)
                {
                    // get all orders from that timeslot
                    var nextOrders = orders.Where(o => o.TimeSlot == firstTimeSlot.TimeSlot);
                    return Json(nextOrders.ToDataSourceResult(request));
                }
                else
                {
                    return Json(string.Empty);
                }                
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
        #endregion
    }
}
