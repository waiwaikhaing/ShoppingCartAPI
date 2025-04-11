using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.EFDBContext
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDBContext()
        {
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemDetailDTO> CartItemViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItemDetailDTO>()
                .HasNoKey() 
                .ToView("vw_CartItemDetails");

            modelBuilder.Entity<Product>()
               .HasKey(c => c.ProductId);

            modelBuilder.Entity<UserData>()
               .HasKey(c=> c.UserId);

            modelBuilder.Entity<CartItem>()
             .HasKey(c => c.CartItemId);
        }

    }
}
