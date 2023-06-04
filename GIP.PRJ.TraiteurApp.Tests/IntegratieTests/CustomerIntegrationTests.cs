using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Repository;
using GIP.PRJ.TraiteurApp.Repository.Interface;
using GIP.PRJ.TraiteurApp.Services;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Telerik.SvgIcons;

namespace GIP.PRJ.TraiteurApp.Tests.IntegratieTests
{
    [TestClass]
    public class CustomerIntegrationTests
    {
        private TraiteurAppDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ClaimsPrincipal _user;
        private CustomersController _controller;

        [TestInitialize]
        public void Initialize()
        {
            // Create an in-memory SQLite database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Initialize the DbContext using the SQLite connection
            _context = new TraiteurAppDbContext(new
            DbContextOptionsBuilder<TraiteurAppDbContext>()
            .UseSqlite(connection)
            .Options);

            // Create the database schema
            _context.Database.EnsureCreated();

            // Mocks
            var userManagerMock = new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);
            var userMock = new Mock<ClaimsPrincipal>();

            // Create the controller instance with the required dependencies
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) => new IdentityUser { Id = userId });
            userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(x => x.Identity.Name).Returns("testuser");
            userMock.Setup(x => x.FindFirst(ClaimTypes.Role)).Returns(new
            Claim(ClaimTypes.Role, "Administrator"));
            _userManager = userManagerMock.Object;
            _user = userMock.Object;
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }
        [TestMethod]
        public async Task AddCustomer_ValidData_Success()
        {
            /*var mockCustomerService = new Mock<ICustomerService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockMailService = new Mock<IMailService>();
            var mockCustomerRepository = new Mock<ICustomerRespository>();*/
            // Arrange
            var customerController = new CustomersController(new
            CustomerService(_context), new OrderService(_context), _userManager, new MailService(), new CustomerRepository(_context))
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = _user }
                }
            };
            var newCustomer = new Customer
            {
                Name = "Customer1",
                EmailAddress = "customer.nr1@ucll.be"
            };

            // Act
            var result = await customerController.Create(newCustomer);


            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);


            // Verify that the newCustomer object is created in the DbContext
            var savedCustomer = await _context.Customers.FirstOrDefaultAsync(c =>
            c.Name == "Customer1");
            Assert.IsNotNull(savedCustomer);
            Assert.AreEqual("customer.nr1@ucll.be", savedCustomer.EmailAddress);
        }
    }
}
