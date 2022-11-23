using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // create a N:N connecting table to allow for an order to contain many products, and a product to exist in many different orders.
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders);



        modelBuilder.Entity<User>().HasData(
            new {
                UserId = Guid.NewGuid().ToString(),
                Email = "JohnDoe@gmail.com",
                UserName = "JohnnyD#1",
                Password = "J0hnD03#123!",
                IsActive = true
                //Orders = new List<Order>(new List<Order>() = new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.UtcNow, Total = 29.99d, IsProcessed = false }, { });
            },
            new {
                UserId = Guid.NewGuid().ToString(),
                Email = "MarySue@hotmail.com",
                UserName = "MarySue#22",
                Password = "M4rySu3san#22!",
                IsActive = true,
                Orders = new Order[] { new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.UtcNow, Total = 29.99m, IsProcessed = false }, new Order { OrderId = Guid.NewGuid().ToString(), OrderDate = DateTime.Parse("2022-10-05 13:27:00"), ShippingDate = DateTime.Parse("2022-11-04 15:27:00"), Total = 15.99m, IsProcessed = true } }
            }
            );

        modelBuilder.Entity<Product>().HasData(
            new { 
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "Universal audio adapter",
                Price = 24.99d
            },
            new {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = "Samsung 55' OLED Q90R 4K Television",
                Price = 1244.99d
            }
            );

        modelBuilder.Entity<Order>().HasData(
            new {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Parse("2022-10-15 22:12:45"),
                Total = 11.99d,
                IsProcessed = false
            },
            new {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.UtcNow,
                ShippingDate = DateTime.Parse("2022-11-04 15:27:00"),
                Total = 44.99d,
                IsProcessed = true,
                Products = new Product[] { new Product { ProductId = Guid.NewGuid().ToString(), ProductName = "Universal audio adapter", Price = 24.99m }, new Product { ProductId = Guid.NewGuid().ToString(), ProductName = "Apple lightning cable 2m", Price = 9.99m } }
            }
            );

        modelBuilder.Entity<Review>().HasData(
            new {
                ReviewId = Guid.NewGuid().ToString(),
                Content = "This video cable splitter is fantastic; it's reliable and works very well :)",
                PostedOn = DateTime.Parse("2022-10-05 13:27:00")
            });

    }
}