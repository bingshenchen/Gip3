using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
/*using GIP.PRJ.TraiteurApp.Repository.Interface;*/
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

namespace GIP.PRJ.TraiteurApp.Tests.ControllerTests
{
    [TestClass]
    public class CookControllerTests
    {
        private CooksController _controller;
        [TestInitialize]
        public void Initialize()
        {
            // Setup an in-memory SQLite database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TraiteurAppDbContext>()
                .UseSqlite(connection)
                .Options;

            // Configure any additional dependencies for the controller (e.g.,services, repositories)
            var mockCookService = new Mock<ICookService>();
            var mockOrderService = new Mock<IOrderService>();

            //Configure mockServices as needed
            var cookNr1 = new Cook { Id = 1, ChefName = "John Doe" };
            mockCookService.Setup(x =>
            x.GetCookByIdAsync(cookNr1.Id)).ReturnsAsync(cookNr1);
            var cookNr2 = new Cook { Id = 2, ChefName = "Jane Doe" };
            mockCookService.Setup(x => x.GetAllCooksAsync()).ReturnsAsync(
            new List<Cook> { cookNr1, cookNr2 });

            var userManagerMock = new Mock<UserManager<IdentityUser>>(new
                Mock<IUserStore<IdentityUser>>().Object,
                null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            //Create the controller instance with the required dependencies
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(x => x.Identity.IsAuthenticated).Returns(true);
            user.Setup(x => x.Identity.Name).Returns("testuser");
            user.Setup(x => x.FindFirst(ClaimTypes.Role)).Returns(new
            Claim(ClaimTypes.Role, "Administrator"));

            _controller = new CooksController(
                mockCookService.Object,
                userManagerMock.Object,
                mockOrderService.Object
              
            )
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user.Object }
                }
            };
        }
        [TestCleanup]
        public void Cleanup()
        {
            _controller.Dispose();
        }
        [TestMethod]
        public async Task Details_ReturnsViewResultWithCook()
        {
            //Arrange
            //service objects setup in the Initialize method
            try
            {
                //Act
                var result = await _controller.Details(1);
                //Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var viewResult = (ViewResult)result;
                //check the viewresult model
                Assert.IsTrue(viewResult.Model is Cook cook && cook.Id ==
                    1);
            }
            catch (AssertFailedException)
            {
                Assert.Fail("The Details result is no ViewResult!");
            }
        }
    }
}