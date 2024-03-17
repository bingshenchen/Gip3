using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
    public class CookIntegrationTests
    {
        private TraiteurAppDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ClaimsPrincipal _user;

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
        public async Task AddCook_ValidData_Succes()
        {
            //Arrange
            var cookController = new CooksController(new
            CookService(_context), _userManager, new OrderService(_context))
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = _user }
                }
            };
            var newCook = new Cook
            {
                ChefName = "Chef1",
                YearsOfExperience = 1
            };

            //Act
            var result = await cookController.Create(newCook);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
            
            //Verify that the newCook object is created in the DbContext
            var savedCook = await _context.Cooks.FirstOrDefaultAsync(c =>
            c.ChefName == "Chef1");
            Assert.IsNotNull(savedCook);
            Assert.AreEqual(1, savedCook.YearsOfExperience);
        }
    }
}
