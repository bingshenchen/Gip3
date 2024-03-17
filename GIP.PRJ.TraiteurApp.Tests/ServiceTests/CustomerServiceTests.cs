using GIP.PRJ.TraiteurApp.Controllers;
using GIP.PRJ.TraiteurApp.Models;
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
    public class CustomerServiceTests
    {
        private TraiteurAppDbContext _context;
        private CustomerService _customerService;
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
            _customerService = new CustomerService(_context);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        public async Task CreateCustomer_Success()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test" };
            // Act
            await _customerService.CreateCustomerAsync(customer);
            // Assert

        var createdCustomer = await _context.Customers.FirstOrDefaultAsync(o =>
        o.Id == customer.Id);
            Assert.IsNotNull(createdCustomer);
            // Additional assertions on the created customer properties
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateCustomer_InvalidData()
        {
            // Arrange
            var invalidCustomer = new Customer { };
            // Act
            await _customerService.CreateCustomerAsync(invalidCustomer);
            // Assert
            // Exception is expected to be thrown
        }
        public async Task GetCustomerById_Success()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            // Act
            var retrievedCustomer = await
            _customerService.GetCustomerByIdAsync(customer.Id);
            // Assert
            Assert.IsNotNull(retrievedCustomer);
            Assert.AreEqual(customer.Id, retrievedCustomer.Id);
        }
        [TestMethod]
        public async Task GetCustomerById_CustomerNotFound()
        {
            // Arrange
            var nonExistentCustomerId = 123;
            // Act
            var retrievedCustomer = await
            _customerService.GetCustomerByIdAsync(nonExistentCustomerId);
            // Assert
            Assert.IsNull(retrievedCustomer);
        }
        public async Task UpdateCustomer_Success()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "No name??" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            customer.Name = "Name updated!";

            // Act
        await _customerService.UpdateCustomerAsync(customer);
            // Assert
            var retrievedCustomer = await _context.Customers.FirstOrDefaultAsync(o =>
            o.Id == customer.Id);
            Assert.IsNotNull(retrievedCustomer);
            Assert.AreEqual(customer.Name, "Name updated!");
        }

    }
}

