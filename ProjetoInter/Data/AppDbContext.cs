using ProjetoInter.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoInter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MarketCarModel> MarketCars { get; set;}
    }
}
