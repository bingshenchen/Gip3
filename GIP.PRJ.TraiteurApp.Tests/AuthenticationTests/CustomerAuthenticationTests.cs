using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
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

namespace GIP.PRJ.TraiteurApp.Tests.AuthenticationTests
{
    [TestClass]
    public class CustomerRoleIdentityTests
    {
        private CustomersController _controller;
        [TestInitialize]
        public void Initialize()
        {
            // Setup an in-memory SQLite database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TraiteurAppDbContext>()
            .UseSqlite(connection)
            .Options;
            // Configure any additional dependencies for the controller (e.g. services, repositories)
            var mockCustomerService = new Mock<ICustomerService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockMailService = new Mock<IMailService>();
            // Configure mockService as needed
            var customerNr1 = new Customer { Id = 1, Name = "John Doe" };
            mockCustomerService.Setup(x =>
           x.GetCustomerByIdAsync(customerNr1.Id)).ReturnsAsync(customerNr1);
            var customerNr2 = new Customer { Id = 2, Name = "Jane Doe" };
            mockCustomerService.Setup(x => x.GetAllCustomersAsync()).ReturnsAsync(
            new List<Customer> { customerNr1, customerNr2 });
            var userManagerMock = new Mock<UserManager<IdentityUser>>(new
           Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) => new IdentityUser { Id = userId });
            // Create test users
            var adminUser = new IdentityUser
            {
                UserName = "adminuser@ucll.be",
                Email
           = "adminuser@ucll.be"
            };
            var customerUser = new IdentityUser
            {
                UserName = "customeruser@ucll.be",
                Email = "customeruser@ucll.be"
            };
            // Create test role
            var adminRole = new IdentityRole { Name = "Administrator" };
            var custRole = new IdentityRole { Name = "Customer" };
            // Add the users to the UserManager
            var users = new List<IdentityUser> { adminUser, customerUser
}.AsQueryable();
            userManagerMock.Setup(u => u.Users).Returns(users);
            // Assign the users to the role
            userManagerMock.Setup(u => u.IsInRoleAsync(adminUser,
           adminRole.Name)).ReturnsAsync(true);
            userManagerMock.Setup(u => u.IsInRoleAsync(customerUser,
           custRole.Name)).ReturnsAsync(true);
            var adminUsers = new List<IdentityUser> { adminUser };
            userManagerMock.Setup(u =>
           u.GetUsersInRoleAsync(adminRole.Name)).ReturnsAsync(adminUsers);
            var customerUsers = new List<IdentityUser> { customerUser };
            userManagerMock.Setup(u =>
           u.GetUsersInRoleAsync(custRole.Name)).ReturnsAsync(customerUsers);
            // Create the controller instance with the required dependencies
            var userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(x => x.Identity.Name).Returns("customerUser");
            userMock.Setup(x => x.IsInRole("Administrator")).Returns(false);
            _controller = new CustomersController(mockCustomerService.Object,
           mockOrderService.Object, userManagerMock.Object, mockMailService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = userMock.Object }
                }
            };
        }
        [TestCleanup]
        public void Cleanup()
        {
            _controller.Dispose();
        }
        public async Task CustomerController_Create_ForbiddenForCustomerRole()
        {
            // Arrange
            // Act
            var result = await _controller.Create();
            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }
    }
}
