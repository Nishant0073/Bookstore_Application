using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Application.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seeding Books
        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = "B1", Title = "C# in Depth", Author = "Jon Skeet", Price = 45.99, CategoryId = "C1", Stock = 100 },
            new Book { BookId = "B2", Title = "Clean Code", Author = "Robert C. Martin", Price = 39.99, CategoryId = "C2", Stock = 50 }
        );

        // Seeding Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = "C1", Name = "Programming", BookId = "B1" },
            new Category { CategoryId = "C2", Name = "Software Development", BookId = "B2" }
        );

        // Seeding Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = "O1", UserId = "U1", OrderDate = DateTime.Now.AddDays(-5), TotalPrice = 85.98 },
            new Order { OrderId = "O2", UserId = "U2", OrderDate = DateTime.Now.AddDays(-3), TotalPrice = 45.99 }
        );

        // Seeding OrderItems
        
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = "OI1", Quantity = 1, Price = 45.99, BookId = "B1", OrderId = "O1" },
            new OrderItem { OrderItemId = "OI2", Quantity = 1, Price = 39.99, BookId = "B2", OrderId = "O1" },
            new OrderItem { OrderItemId = "OI3", Quantity = 1, Price = 45.99, BookId = "B1", OrderId = "O2" }
        );
    }
}