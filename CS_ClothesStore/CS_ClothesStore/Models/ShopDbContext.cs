using Microsoft.EntityFrameworkCore;

namespace CS_ClothesStore.Models
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public ShopDbContext()
        {
        }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<ProductPromotions> ProductPromotions { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductVariants> ProductVariants { get; set; }
        public DbSet<Promotions> Promotions { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Shipments> Shipments { get; set; }
        public DbSet<Sizes> Sizes { get; set; }
        public DbSet<PasswordResets> PasswordResets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Accounts>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Accounts>().HasIndex(a => a.Phone).IsUnique();

            modelBuilder.Entity<ProductPromotions>()
                .HasKey(pp => new { pp.ProductId, pp.PromotionId });

            modelBuilder.Entity<ProductPromotions>()
                .HasOne(pp => pp.Products)
                .WithMany(p => p.ProductPromotions)
                .HasForeignKey(pp => pp.ProductId);

            modelBuilder.Entity<ProductPromotions>()
                .HasOne(pp => pp.Promotions)
                .WithMany(pr => pr.ProductPromotions)
                .HasForeignKey(pp => pp.PromotionId);

           
        }
    }
}
