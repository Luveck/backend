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
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Patology> Patology { get; set; }
        public DbSet<Pharmacy> Pharmacy { get; set; }
        public DbSet<Purchase> Purchase { get; set; }      
        public DbSet<Medical> Medical { get; set; }
        public DbSet<ProductChangeRule> ProductChangeRule { get; set; }
        public DbSet<ProductPurchase> ProductPurchases { get; set; }
        public DbSet<ExchangedProduct> ExchangedProducts { get; set; }
        public DbSet<MassiveRemainder> MassiveRemainders { get; set; }
    }
}
