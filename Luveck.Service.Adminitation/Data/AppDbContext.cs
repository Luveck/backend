using Luveck.Service.Administration.Models;
using Microsoft.EntityFrameworkCore;

namespace Luveck.Service.Administration.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Country> Country { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<SBU> SBU { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
