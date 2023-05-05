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
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using GIP.PRJ.TraiteurApp.Services.DTO;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator")] 
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMailService _mailService;
        private readonly IInvoiceService _invoiceService;
        private readonly ITimeSlotService _timeSlotService;

        public OrdersController(IOrderService orderService, ICustomerService customerService, IOrderDetailService orderDetailService, 
            IMailService mailService, IInvoiceService invoiceService, ITimeSlotService timeSlotService)
        {
            _orderService = orderService;
            _customerService = customerService; 
            _orderDetailService = orderDetailService;
            _mailService = mailService;
            _invoiceService = invoiceService;
            _timeSlotService = timeSlotService;
        }

        // GET: Orders
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CustomerIndex(int id)
        {
            ViewBag.CustomerId = id;
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.CustomerName = customer.Name;
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

            ViewBag.IsLocked = await _timeSlotService.OrderIsLocked(id.Value);
            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(int? id)
        {
            Order order = new Order{ OrderedOn = DateTime.Now };
            if (id != null)
            {
                order.CustomerId = id.Value;
                ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name", order.CustomerId);
            }
            else
            {
                ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name");
            }
            
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
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(order);
                return RedirectToAction("Create", "OrderDetails", new { id = order.Id });
            }

            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name", order.CustomerId);
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

            if (await _timeSlotService.OrderIsLocked(id.Value))
            {
                return RedirectToAction(nameof(Details), order);
            }

            ViewData["TimeSlots"] = new SelectList(await _timeSlotService.GetTimeSlotDictionary(), "Key", "Value");
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name", order.CustomerId);

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
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /// include Customer => to check customer rating (see below)
            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            // calculate total
            var orderDetails = _orderDetailService.GetOrderDetailsByOrderAsync(order.Id);
            decimal total = (await orderDetails).Sum(od => od.Price * od.Quantity);
            order.Total = total;

            // get customer rating to set reduction (if A => 10% reduction)
            if (order.Customer.Rating == CustomerRating.A)
            {
                order.Reduction = 10;
            }
            
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id, [Bind("Id,OrderedOn,TimeSlot,CustomerId,Total,Reduction,IsPaid")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (order.IsPaid)
            {
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /// set customer rating
                    /// 
                    int customerCount = await _orderService.OrderCountByCustomer(order.CustomerId);
                    if (customerCount >= 3)
                    {
                        Customer customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
                        customer.Rating = CustomerRating.A;
                    }

                    /// set order paid
                    order.IsPaid = true;
                    await _orderService.UpdateOrderAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Invoice), new { id = order.Id });
            }

            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomersAsync(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        public async Task<IActionResult> Invoice(int? id)
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

            ViewBag.TimeSlot = order.TimeSlot;
            ViewBag.OrderId = order.Id;
            ViewBag.CustomerInfo = order.Customer.Name + (string.IsNullOrEmpty(order.Customer.EmailAddress) ? string.Empty : " (" + order.Customer.EmailAddress + ")");
            ViewBag.IsProfessional = !string.IsNullOrEmpty(order.Customer.VATNumber);
            InvoiceTotals invoiceTotals = await _invoiceService.CalculateTotals(order.Id, order.Reduction);

            ViewBag.TotalInclVAT = invoiceTotals.TotalInclVAT;
            
            ViewBag.TotalExclVAT = invoiceTotals.TotalExclVAT;
            ViewBag.VAT = invoiceTotals.TotalVAT;
            ViewBag.Reduction = invoiceTotals.TotalReduction;
            ViewBag.ToPay = invoiceTotals.ToPay;

            // include menuitem for name of item

            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderAsync(id.Value);

            return View(orderDetails);
        }

        public async Task<IActionResult> SendMail(int? id)
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

            ViewBag.TimeSlot = order.TimeSlot;
            ViewBag.OrderId = order.Id;
            string customerInfo = order.Customer.Name + (string.IsNullOrEmpty(order.Customer.EmailAddress) ? string.Empty : " (" + order.Customer.EmailAddress + ")");
            string paymentInfo = (order.Total - (order.Total * order.Reduction / 100)) + (order.Reduction > 0 ? "(Korting: " + order.Reduction + "%)" : string.Empty);

            // include menuitem for name of item
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderAsync(id.Value);

            string detailInfo = string.Empty;
            foreach (var orderDetail in orderDetails)
            {
                detailInfo += "<tr>" +
                                    "<td>" + orderDetail.MenuItem.Name + "</td>" +
                                    "<td>" + orderDetail.Quantity + "</td>" +
                                    "<td>" + orderDetail.Price + "</td>" +
                                "</tr>";
            }

            string mailContent =
                "<html><body><h2>Factuur</h2>" +
                "<div><h4>Bestelling - " + order.Id + "</h4>" +
                "<hr>" +
                "<dl>" +
                "<dt>Timeslot</dt>" +
                "<dd>" + order.TimeSlot + "</dd>" +
                "<dt>Klant naam</dt>" +
                "<dd>" + customerInfo + "</dd>" +
                "<dt>Totaal</dt>" +
                "<dd>" + paymentInfo + "</dd>" +
                "</dl>" +
                "<table border=0>" +
                "<thead><tr> <th>MenuItem</th><th>Quantity</th><th>Price</th><th></th></tr></thead>" +
                "<tbody>" + detailInfo +
                "</tbody>" +
                "</table>" +
                "</div>" +
                "</body></html>";

            try
            {
                _mailService.SendMail("from@domain", order.Customer.EmailAddress, "Lekkerbek - Factuur (bestelling " + order.Id + ")",
                    mailContent);
                ViewBag.InfoMessage = "De mail werd correct verstuurd";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Fout tijdens versturen mail (" + ex.Message + ")";
            }

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            if (await _timeSlotService.OrderIsLocked(id.Value))
            {
                return View("NoDelete", order);
            }

            return View(order); 
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task<bool> OrderExists(int id)
        {
            return (await _orderService.GetOrderByIdAsync(id)) != null;
        }

        #region Kendo
        public async Task<IActionResult> GetOrders([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();

                return Json(orders.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }

        public async Task<IActionResult> GetOrdersByCustomer([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var orders = await _orderService.GetOrdersByCustomerAsync(id);

                return Json(orders.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
        #endregion
    }
}
