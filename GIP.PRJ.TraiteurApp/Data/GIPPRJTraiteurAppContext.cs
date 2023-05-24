using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Data
{
    public class GIPPRJTraiteurAppContext : DbContext
    {
        public GIPPRJTraiteurAppContext (DbContextOptions<GIPPRJTraiteurAppContext> options)
            : base(options)
        {
        }

        public DbSet<GIP.PRJ.TraiteurApp.Models.CreateRolesViewModel> CreateRolesViewModel { get; set; } = default!;
    }
}
