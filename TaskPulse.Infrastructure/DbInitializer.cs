using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPulse.Infrastructure
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Core.Entities.Product { Name = "Coca-cola 0.5L", Price = 75, Stock = 50},
                    new Core.Entities.Product { Name = "Pepsi 0.5L", Price = 70, Stock = 40 },
                    new Core.Entities.Product { Name = "Sprite 0.5L", Price = 72, Stock = 30 }
                );
                db.SaveChanges();
            }
        }
    }
}
