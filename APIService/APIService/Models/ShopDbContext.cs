using Microsoft.EntityFrameworkCore;

namespace APIService.Models
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
        public DbSet<EmailVerification> EmailVerification { get; set; }
        public DbSet<ShippingProviders> ShippingProviders { get; set; }

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

            

            // --- ACCOUNTS ---
            modelBuilder.Entity<Accounts>().HasData(
                new Accounts { Id = 1, Fullname = "Truong Tran Long", Email = "truongtranlong23@gmail.com", Password = "$2a$12$cVw6vAlKHuxIFsz32ElSnOmMjjoeFpyIbRHZOfrim7nAgz0UbImI6", Phone = "0868984121", RoleId = 1, Address="Vinh Long", Gender = "Male", BirthDay = new DateOnly(2025, 10, 1), CreateAt = DateOnly.FromDateTime(DateTime.Now), UpdateAt = DateOnly.FromDateTime(DateTime.Now) },
                new Accounts { Id = 2, Fullname = "Tran Thi B", Email = "longttce171365@fpt.edu.vn", Password = "$2a$12$cVw6vAlKHuxIFsz32ElSnOmMjjoeFpyIbRHZOfrim7nAgz0UbImI6", Phone = "0868984122", RoleId = 4, Address = "Vinh Long", Gender = "Male", BirthDay = new DateOnly(2025, 10, 1), CreateAt = DateOnly.FromDateTime(DateTime.Now), UpdateAt = DateOnly.FromDateTime(DateTime.Now) },
                new Accounts { Id = 3, Fullname = "Phạm Văn C", Email = "customer@shop.com", Password = "$2a$12$cVw6vAlKHuxIFsz32ElSnOmMjjoeFpyIbRHZOfrim7nAgz0UbImI6", Phone = "0868984123", RoleId = 5, Address = "Vinh Long", Gender = "Male", BirthDay = new DateOnly(2025, 10, 1), CreateAt = DateOnly.FromDateTime(DateTime.Now), UpdateAt = DateOnly.FromDateTime(DateTime.Now) }
            );

            // --- CART ITEMS ---
            modelBuilder.Entity<CartItems>().HasData(
                new CartItems { Id = 1, CartId = 1, ProductVariantId = 1, Quantity = 2 },
                new CartItems { Id = 2, CartId = 1, ProductVariantId = 3, Quantity = 1 }
            );

            // --- CARTS ---
            modelBuilder.Entity<Carts>().HasData(
                new Carts { Id = 1, AccountId = 3, CreatedAt = DateTime.Now }
            );

            // --- PAYMENTS ---
            modelBuilder.Entity<Payments>().HasData(
                new Payments
                {
                    Id = 1,
                    Amount = 500000,
                    OrderId = 1,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "COD",
                    PaymentStatus = "Pending"
                },
                new Payments
                {
                    Id = 2,
                    Amount = 500000,
                    OrderId = 1,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "Bank Transfer",
                    PaymentStatus = "Completed"
                }
            );

            // --- CATEGORIES ---
            modelBuilder.Entity<Categories>().HasData(
                new Categories { Id = 1, CategoryName = "Shirt" },
                new Categories { Id = 2, CategoryName = "Pants" },
                new Categories { Id = 3, CategoryName = "Dress" },
                new Categories { Id = 4, CategoryName = "Accressory" }
            );

            // --- COLORS ---
            modelBuilder.Entity<Colors>().HasData(
                new Colors { Id = 1, ColorName = "White" },
                new Colors { Id = 2, ColorName = "Black" },
                new Colors { Id = 3, ColorName = "Red" },
                new Colors { Id = 4, ColorName = "Blue" }
            );

            // --- ORDERS ---
            modelBuilder.Entity<Orders>().HasData(
                new Orders
                {
                    Id = 1,
                    AccountId = 3,
                    OrderDate = DateTime.Now,
                    TotalPrice = 500000,
                    Status = "Pending",
                    ShippingAddress = "123 Main St",
                    PaymentMethod = "COD",
                    Note = "First order"
                }
            );

            // --- ORDER DETAILS ---
            modelBuilder.Entity<OrderDetails>().HasData(
                new OrderDetails
                {
                    Id = 1,
                    OrderId = 1,
                    ProductVariantsId = 1,
                    Quantity = 2,
                    UnitPrice = 250000
                }
            );

            // --- PRODUCTS ---
            modelBuilder.Entity<Products>().HasData(
                new Products
                {
                    Id = 1,
                    ProductName = "White Shirt",
                    CategoryId = 1,
                    Price = 250000,
                    Description = "Áo sơ mi trắng tay dài",
                    Discount = 0,
                    Image = "product-02.jpg",
                    StockQuantity = 10,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = new DateTime(2025, 10, 1)
                },
                new Products
                {
                    Id = 2,
                    ProductName = "Blue Jeans",
                    CategoryId = 2,
                    Price = 400000,
                    Description = "Quần jean nam ống đứng",
                    Discount = 0,
                    Image = "product-07.jpg",
                    StockQuantity = 50,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = new DateTime(2025, 10, 1)
                },
                new Products
                {
                    Id = 3,
                    ProductName = "Summer Dress",
                    CategoryId = 3,
                    Price = 320000,
                    Description = "Váy hoa vintage",
                    Discount = 0,
                    Image = "product-05.jpg",
                    StockQuantity = 32,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = new DateTime(2025, 10, 1)
                },
                new Products
                {
                    Id = 4,
                    ProductName = "Black T-Shirt",
                    CategoryId = 1,
                    Price = 180000,
                    Description = "Áo thun cotton",
                    Discount = 0,
                    Image = "product-14.jpg",
                    StockQuantity = 54,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = new DateTime(2025, 10, 1)
                },
                new Products
                {
                    Id = 5,
                    ProductName = "Leather Jacket",
                    CategoryId = 1,
                    Price = 750000,
                    Description = "Áo khoác da cao cấp",
                    Discount = 0,
                    Image = "product-04.jpg",
                    StockQuantity = 11,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = new DateTime(2025, 10, 1)
                }
            );

            // --- PRODUCT VARIANTS ---
            modelBuilder.Entity<ProductVariants>().HasData(
                new ProductVariants
                {
                    Id = 1,
                    ProductId = 1,
                    ColorId = 1,
                    SizeId = 2,
                    Stock = 30,
                    Price = 250000
                },
                new ProductVariants
                {
                    Id = 2,
                    ProductId = 2,
                    ColorId = 4,
                    SizeId = 3,
                    Stock = 25,
                    Price = 400000
                },
                new ProductVariants
                {
                    Id = 3,
                    ProductId = 3,
                    ColorId = 3,
                    SizeId = 2,
                    Stock = 15,
                    Price = 320000
                },
                new ProductVariants
                {
                    Id = 4,
                    ProductId = 4,
                    ColorId = 2,
                    SizeId = 1,
                    Stock = 50,
                    Price = 180000
                },
                new ProductVariants
                {
                    Id = 5,
                    ProductId = 5,
                    ColorId = 2,
                    SizeId = 4,
                    Stock = 10,
                    Price = 750000
                }
            );

            // --- PROMOTIONS ---
            modelBuilder.Entity<Promotions>().HasData(
                new Promotions { Id = 1, Code = "SALE10", DiscountPercent = 10, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 10, 31), IsActive = "Active" },
                new Promotions { Id = 2, Code = "SALE20", DiscountPercent = 20, StartDate = new DateTime(2025, 11, 1), EndDate = new DateTime(2025, 11, 30), IsActive = "Active" }
            );

            // --- ROLES ---
            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, RoleName = "ADMIN" },
                new Roles { Id = 2, RoleName = "MANAGER" },
                new Roles { Id = 3, RoleName = "CONSULTING_STAFF" },
                new Roles { Id = 4, RoleName = "SALE_STAFF" },
                new Roles { Id = 5, RoleName = "CUSTOMER" }
            );

            // --- PRODUCT PROMOTIONS ---
            modelBuilder.Entity<ProductPromotions>().HasData(
                new ProductPromotions { ProductId = 1, PromotionId = 1 },
                new ProductPromotions { ProductId = 2, PromotionId = 1 },
                new ProductPromotions { ProductId = 3, PromotionId = 2 },
                new ProductPromotions { ProductId = 4, PromotionId = 2 },
                new ProductPromotions { ProductId = 5, PromotionId = 2 }
            );

            // --- SIZES ---
            modelBuilder.Entity<Sizes>().HasData(
                new Sizes { Id = 1, SizeName = "S" },
                new Sizes { Id = 2, SizeName = "M" },
                new Sizes { Id = 3, SizeName = "L" },
                new Sizes { Id = 4, SizeName = "XL" }
            );

                modelBuilder.Entity<ShippingProviders>().HasData(
            new ShippingProviders
            {
                Id = 1,
                Name = "Express",
                Description = "Fast and reliable delivery service for all regions.",
                DefaultFee = 25000.00m,
                ContactNumber = "19001234"
            },
            new ShippingProviders
            {
                Id = 2,
                Name = "GHTK",
                Description = "Popular Vietnamese shipping provider offering nationwide delivery.",
                DefaultFee = 30000.00m,
                ContactNumber = "19006092"
            }
        );
        }
    }
}
