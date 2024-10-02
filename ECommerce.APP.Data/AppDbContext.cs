using ECommerce.APP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.APP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
