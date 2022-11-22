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
        public DbSet<Patology> Patology { get; set; }
        public DbSet<Pharmacy> Pharmacy { get; set; }
        public DbSet<Purchase> Purchase { get; set; }      
        public DbSet<User> User { get; set; }
        public DbSet<Medical> Medical { get; set; }
    }
}
