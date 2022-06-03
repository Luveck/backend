using Luveck.Service.Security.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Luveck.Service.Security.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> user { get; set; }
        public DbSet<Module> modules { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
    }
}
