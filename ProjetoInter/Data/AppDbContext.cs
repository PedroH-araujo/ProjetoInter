using ProjetoInter.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoInter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>()
               .HasKey(product =>  product.Id);  // Define a chave primária

            modelBuilder.Entity<ProductModel>()
               .Property(product => product.Title)
               .HasMaxLength(100);

            modelBuilder.Entity<ProductModel>()
                .Property(product => product.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<ProductModel>()
                .HasOne(product => product.Seller)               // Um produto tem um vendedor
                .WithMany(user => user.ProductsSold)       // Um vendedor pode ter vários produtos
                .HasForeignKey(product => product.SellerId);     // Chave estrangeira para SellerId na tabela de produtos

            modelBuilder.Entity<UserModel>()
               .HasKey(user => user.Id);    // Define a chave primária

            modelBuilder.Entity<UserModel>()
                .Property(user => user.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<UserModel>()
                .Property(user => user.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<UserModel>()
                .Property(user => user.Address)
                .HasMaxLength(100);

            modelBuilder.Entity<UserModel>()
                .Property(user => user.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<UserModel>()
               .HasMany(user => user.ProductsSold)      // Um usuário pode ter vários produtos vendidos
               .WithOne(product => product.Seller)            // Um produto pertence a um único vendedor
               .HasForeignKey(product => product.SellerId);   // Chave estrangeira para SellerId na tabela de produtos

            modelBuilder.Entity<MarketCarModel>()
                .HasKey(marketCar => marketCar.Id);  // Define a chave primária

            modelBuilder.Entity<MarketCarModel>()
                .HasOne(marketCar => marketCar.Product)  // Um MarketCarModel tem um produto associado
                .WithMany(product => product.MarketCars) // Um produto pode estar em vários MarketCarModels
                .HasForeignKey(marketCar => marketCar.ProductId);  // Chave estrangeira para ProductId na tabela de MarketCarModels

            modelBuilder.Entity<MarketCarModel>()
                .HasOne(marketCar => marketCar.User)  // Um MarketCarModel pertence a um usuário
                .WithMany(user => user.MarketCars)  // Um usuário pode ter vários MarketCarModels
                .HasForeignKey(marketCar => marketCar.UserId);  // Chave estrangeira para UserId na tabela de MarketCarModels

        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MarketCarModel> MarketCars { get; set;}
    }
}
