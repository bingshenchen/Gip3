using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP.PRJ.TraiteurApp.Tests.ServiceTests
{
    public class CustomerServiceTests
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
            // use _context where needed

            //mocks
            

        }
        [TestMethod]
        public void Customer_When_Customer()
        {
            var mockCustomerService = new Mock<ICustomerService>();
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

