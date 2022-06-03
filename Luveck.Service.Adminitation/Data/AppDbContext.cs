using Luveck.Service.Administration.Models;
using Microsoft.EntityFrameworkCore;

namespace Luveck.Service.Administration.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Country> Countries { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<SBU> Sbu { get; set; }
    }
}
