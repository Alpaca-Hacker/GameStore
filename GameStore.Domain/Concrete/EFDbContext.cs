using GameStore.Domain.Entities;
using System.Data.Entity;

namespace GameStore.Domain.Concrete
{
    public class EFDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
