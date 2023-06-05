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

namespace GIP.PRJ.TraiteurApp.Tests.ServiceTests
{
    [TestClass]
    public class CookServiceTests
    {
        private TraiteurAppDbContext _context;
        private CookService _cookService;
        [TestInitialize]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<TraiteurAppDbContext>()
            .UseSqlite(connection)
            .Options;
            _context = new TraiteurAppDbContext(options);
            _context.Database.EnsureCreated();
            _cookService = new CookService(_context);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [TestMethod]
        public async Task CreateCook_Success()
        {
            //Arrange
            var cook = new Cook { Id = 1, ChefName = "Test" };
            //Act
            await _cookService.CreateCookAsync(cook);
            //Assert
            var createdCook = await _context.Cooks.FirstOrDefaultAsync(c =>
            c.Id == cook.Id);
            Assert.IsNotNull(createdCook);
            // Additional assertions on the created cook properties
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateCook_InvalidData()
        {
            //Arrange 
            var invalidCook = new Cook { };
            //Act
            await _cookService.CreateCookAsync(invalidCook);
            //Assert
            //Exception is expected to be thrown
        }
        [TestMethod]
        public async Task GetCookById_Succes()
        {
            // Arrange
            var cook = new Cook { Id = 1, ChefName = "Test" };
            _context.Cooks.Add(cook);
            await _context.SaveChangesAsync();
            //Act
            var retrievedCook = await
                _cookService.GetCookByIdAsync(cook.Id);
            //Assert
            Assert.IsNotNull(retrievedCook);
            Assert.AreEqual(cook.Id, retrievedCook.Id);
        }
        [TestMethod]
        public async Task GetCookById_CustomerNotFound()
        {
            //Arrange
            var nonExistendCookId = 123;
            //Act
            var retrievedCook = await 
            _cookService.GetCookByIdAsync(nonExistendCookId);
            //Assert
            Assert.IsNull(retrievedCook);
        }
        [TestMethod]
        public async Task UpdateCook_Succes()
        {
            //Arrange
            var cook = new Cook { Id = 1, ChefName = "No name??" };
            _context.Cooks.Add(cook);
            await _context.SaveChangesAsync();
            cook.ChefName = "Name updated!";

            //Act
            await _cookService.UpdateCookAsync(cook);
            //Assert
            var retrievedCook = await _context.Cooks.FirstOrDefaultAsync(c =>
            c.Id == cook.Id);
            Assert.IsNotNull(retrievedCook);
            Assert.AreEqual(cook.ChefName, "Name updated!");
        }
            
    }
}