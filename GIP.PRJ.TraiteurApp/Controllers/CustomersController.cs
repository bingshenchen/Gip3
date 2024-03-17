using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using GIP.PRJ.TraiteurApp.Services;
using System.Drawing;

namespace GIP.PRJ.TraiteurApp.Controllers
{
    [Authorize(Roles = "Administrator, Customer")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;
        private ICustomerService object1;
        private IOrderService object2;
        private UserManager<IdentityUser> object3;

        public CustomersController(ICustomerService customerService, IOrderService orderService, UserManager<IdentityUser> userManager, IMailService mailService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _userManager = userManager;
            _mailService = mailService;
        }

        /*public CustomersController(ICustomerService object1, IOrderService object2, UserManager<IdentityUser> object3)
        {
            this.object1 = object1;
            this.object2 = object2;
            this.object3 = object3;
        }*/

        //REPOSITORY CODE ZIJN ALLEMAAL IN COMENTAAR GEZET!!
        // GET: Customers
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            /*var data = _respository.GetCustomers();
            return View(data);*/
            return View();
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Customer customer1 = _respository.GetCustomerById(id);
            return View(customer1);*/

            Customer customer;

            if (User.IsInRole("Customer"))
            {
                customer = await _customerService.GetCustomerByIdentityAsync(_userManager.GetUserId(User));
            }
            else
            {
                customer = await _customerService.GetCustomerByIdAsync(id.Value);
            }

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.IdentityUsers = new SelectList(users, "Id", "UserName");

            return View(new Customer { Rating = CustomerRating.B });
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Rating,Info,EmailAddress,IdentityUserId,CompanyName,VATNumber,Address")] Customer customer)
        {
            /*_respository.CreateCustomer(customer);
            _respository.Save();
            return RedirectToAction("Index");*/

            if (ModelState.IsValid)
            {
                try
                {
                    // Hier kan switch of if gebruiken voor de verschillende klanten 
                    var mailContent = 
                      /*  $"Beste {customer.Name},\n\nBedankt voor het registreren van een account bij LekkerBek! We zijn verheugd om je als nieuwe gebruiker te verwelkomen in onze community.\n\nMet vriendelijke groeten\n\nTeam 21";
                    _mailService.SendMail("lekkerbekgip3@outlook.com", customer.EmailAddress, "Lekkerbek - Bevestiging aanmelding (klant " + customer.Name + ")",
                         mailContent);*/

                    "<html><body>" +
                    "<div>" + "Beste " + customer.Name + "</div>" +
                    "<br>" +
                    "<div>" + "Dit is een bevestigingsmail voor je aanmelding" + "</div>" +
                    "<br>" +
                    "<div>" + "Met vriendelijke groeten" + "</div>" +
                    "<div>" + "Groep 21" + "</div>" +
                    "</body></html>";
                    _mailService.SendMail("lekkerbekgip3@outlook.com", customer.EmailAddress, "Lekkerbek - Bevestiging aanmelding (klant " + customer.Name + ")",
                         mailContent);
                    //ViewBag.InfoMessage = "De mail werd correct verstuurd";
                    TempData["InfoMessage"] = "De mail werd correct verstuurd";
                }
                catch (Exception ex)
                {
                    //ViewBag.ErrorMessage = "Fout tijdens versturen mail (" + ex.Message + ")";
                    TempData["ErrorMessage"] = "Fout tijdens versturen mail (" + ex.Message + ")";
                }
                await _customerService.CreateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.IdentityUsers = new SelectList(users, "Id", "UserName");
            return View(customer);
        }

        // GET: Customers/Edit/5        
        public async Task<IActionResult> Edit(int? id)
        {
            /*Customer customer = _respository.GetCustomerById(id);
            return View(customer);*/

            Customer customer;
            if (User.IsInRole("Customer"))
            {
                customer = await _customerService.GetCustomerByIdentityAsync(_userManager.GetUserId(User));
            }
            else
            {
                customer = await _customerService.GetCustomerByIdAsync(id.Value);
            }

            if (customer == null)
            {
                return NotFound();
            }

            var users = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.IdentityUsers = new SelectList(users, "Id", "UserName");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rating,Info,EmailAddress," +
            "IdentityUserId,CompanyName,VATNumber,Address")] Customer customer)
        {
            /*_respository.UpdateCustomer(customer);
            _respository.Save();
            return RedirectToAction(nameof(Index));*/

            if (id != customer.Id)
            {
                return NotFound();
            }

            if (User.IsInRole("Customer"))
            {
                var identityId = _userManager.GetUserId(User);
                if (customer.IdentityUserId != identityId)
                {
                    return NotFound();
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.UpdateCustomerAsync(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (User.IsInRole("Customer"))
                {
                    return RedirectToAction(nameof(Details));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            var users = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.IdentityUsers = new SelectList(users, "Id", "UserName");
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService?.GetCustomerByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            if ((await _orderService.OrderCountByCustomer(id.Value)) > 0)
            {
                return View("NoDelete", customer);
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CustomerExists(int id)
        {
            return (await _customerService.GetCustomerByIdAsync(id)) != null;
        }

        #region Kendo 
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetCustomers([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                // data if Administrator
                var customers = await _customerService.GetAllCustomersAsync();
                return Json(customers.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(string.Empty);
            }
        }
        #endregion
    }
}
