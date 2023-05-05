using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Models
{
    public class TraiteurAppDbContext : IdentityDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }

        public TraiteurAppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MenuItemCategory>().HasData(
                new MenuItemCategory { Id = -1, Name = "Afhaalgerechten", VAT = 6 },
                new MenuItemCategory { Id = -2, Name = "Niet-alcoholische dranken", VAT = 6 },
                new MenuItemCategory { Id = -3, Name = "Alcoholische dranken", VAT = 21}
                );
        }
    }
}
