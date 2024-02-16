using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotSauce.Models
{
    public class HotSauceContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Sauce> Sauces { get; set; }
        public DbSet<Flavor> Flavors { get; set; }
        public DbSet<FlavorSauce> FlavorSauces { get; set; }

        public HotSauceContext(DbContextOptions options) : base(options) { }
    }
}