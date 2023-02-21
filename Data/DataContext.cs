using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class DataContext : DbContext
{
    private readonly bool _addBasicEntities = false;
    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>(u =>
        {
            u.HasKey(u => u.UserId);
            u.Property(u => u.UserId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Product>(p =>
        {
            p.HasKey(p => p.ProductId);
            p.Property(p => p.ProductId).ValueGeneratedOnAdd();

            p.Property(p => p.Price).HasColumnType("decimal");
            p.Property(p => p.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.HasKey(o => o.OrderId);
            o.Property(o => o.OrderId).ValueGeneratedOnAdd();

            o.HasMany(o => o.Products).WithMany(p => p.Orders);

            o.Property(o => o.Total).HasColumnType("decimal");
            o.Property(o => o.Total).HasPrecision(18, 2);
        });

        /*modelBuilder.Entity<OrderProduct>(op =>
        {
            op.HasKey(op => op.OrderProductId);
            op.Property(o => o.OrderProductId).ValueGeneratedOnAdd();
        });*/

        modelBuilder.Entity<Review>(r =>
        {
            r.HasKey(r => r.ReviewId);
            r.Property(r => r.ReviewId).ValueGeneratedOnAdd();

            //r.HasOne(r => r.ProductId).WithMany(p => p.Reviews);
        });

        if (_addBasicEntities)
        {

            modelBuilder.Entity<User>().HasData(
            new
            {
                UserId = new Guid("a75e3fe7-f519-48de-a106-79f788a1b479").ToString(),
                Email = "JohnDoe@gmail.com",
                UserName = "JohnnyD#1",
                Password = "J0hnD03#123!",
                //Orders = new List<Order>(new List<Order>() = new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.UtcNow, Total = 29.99d, IsProcessed = false }, { });
            },
            new
            {
                UserId = new Guid("383ee594-db7d-41c5-8766-e63fe76cb9d9").ToString(),
                Email = "MarySue@hotmail.com",
                UserName = "MarySue#22",
                Password = "M4rySu3san#22!",
                //Orders = new Order[] { new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.UtcNow, ShippingDate = DateTime.Parse("2023-03-03 09:00:00"), Total = 29.99m, IsProcessed = false }, new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.Parse("2022-10-05 13:27:00"), ShippingDate = DateTime.Parse("2022-11-04 12:00:00"), Total = 15.99m, IsProcessed = true } }
            }
            );

            modelBuilder.Entity<Product>().HasData(
                new
                {
                    ProductId = new Guid("43ccb5a5-56a6-4182-b318-243d200d9a30").ToString(),
                    ProductName = "Universal audio adapter",
                    Price = 24.99m
                },
                new
                {
                    ProductId = Guid.NewGuid().ToString(),
                    ProductName = "Samsung 55' OLED Q90R 4K Television",
                    Price = 1244.99m
                }
                );

            modelBuilder.Entity<Order>().HasData(
                new
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.UtcNow,
                    ShippingDate = DateTime.UtcNow,
                    Total = 24.99m,
                    IsProcessed = false
                },
                new
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.UtcNow,
                    ShippingDate = DateTime.UtcNow,
                    Total = 44.99m,
                    IsProcessed = true,
                    //Products = new Product[] { new Product { ProductId = new Guid("43ccb5a5-56a6-4182-b318-243d200d9a30").ToString(), ProductName = "Universal audio adapter", Price = 24.99m }, new Product { ProductId = Guid.NewGuid().ToString(), ProductName = "Apple lightning cable 2m", Price = 9.99m } }
                }
                );

            modelBuilder.Entity<Review>().HasData(
                new
                {
                    ReviewId = Guid.NewGuid().ToString(),
                    Content = "This universal audio adapter is fantastic; it's reliable and works very well :)",
                    PostedOn = DateTime.UtcNow,
                    ProductId = new Guid("43ccb5a5-56a6-4182-b318-243d200d9a30").ToString()
                });
        }
    }
}