using Microsoft.EntityFrameworkCore;
using TaskPulse.Core.Entities;

namespace TaskPulse.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
