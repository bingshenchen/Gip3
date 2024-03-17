using GIP.PRJ.TraiteurApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP.PRJ.TraiteurApp.Tests.ModelTests
{
    [TestClass]
        public class CookTests
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
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [TestMethod]
        public void ValidateCookModel_NameIsRequired()
        {
            //Arrange
            var cook = new Cook
            {
                Id = 1,
                ChefName = null,
                YearsOfExperience = 0,
            };
            //Act & Assert
            Assert.ThrowsExceptionAsync<ValidationException>(() =>
            _context.SaveChangesAsync());
        }
    }
}
