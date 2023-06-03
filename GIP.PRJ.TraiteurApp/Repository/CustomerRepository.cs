using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Repository.Interface;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using AspNetCore;
using Microsoft.EntityFrameworkCore;
using Telerik.SvgIcons;

namespace GIP.PRJ.TraiteurApp.Repository
{
    public class CustomerRepository : ICustomerRespository
    {
        private readonly TraiteurAppDbContext _dbContext;

        public CustomerRepository(TraiteurAppDbContext dbContext)
        {     
            _dbContext = dbContext;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _dbContext.Customers.ToList();
        }
        public Customer GetCustomerById(int? id)
        {
            return _dbContext.Customers.Find(id);
        }
        public Customer GetCustomerByIdentityAsync(string id)
        {
            throw new NotImplementedException();
        }
        public void CreateCustomer(Customer c)
        {
            _dbContext.Customers.Add(c);
        }
        public void UpdateCustomer(Customer c)
        {
            _dbContext.Entry(c).State = EntityState.Modified;
        }

        public void DeleteCustomer(int id)
        {
            Customer customer = _dbContext.Customers.Find(id);
            _dbContext.Customers.Remove(customer);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
