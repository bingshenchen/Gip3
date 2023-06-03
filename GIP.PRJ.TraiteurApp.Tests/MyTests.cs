using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP.PRJ.TraiteurApp.Tests
{
    //MIMIC OF THE DATABASE
    [TestClass]
    public class MyTests
    {
        private TraiteurAppDbContext _context;
        [TestInitialize]
        public void Initialize()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TraiteurAppDbContext>()
            .UseSqlite(connection)
            .Options;

            _context = new TraiteurAppDbContext(options);
            _context.Database.EnsureCreated();

        }
        [TestMethod]
        public void TestMethod1()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            //Customer 1

            var customerNr1 = new Customer { Id = 1, Name = "Jhon Doe" };
            mockCustomerService.Setup(x => x.GetCustomerByIdAsync(customerNr1.Id)).ReturnsAsync(customerNr1);

            //Customer 2
            var customerNr2 = new Customer { Id = 2, Name = "Richard Doe" };
            mockCustomerService.Setup(x => x.GetCustomerByIdAsync(customerNr2.Id)).ReturnsAsync(customerNr2);

            //controller Object
            //var controller = new CustomersController(mockCustomerService.Object);



        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}
