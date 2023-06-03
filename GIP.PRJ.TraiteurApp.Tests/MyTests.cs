using GIP.PRJ.TraiteurApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}
