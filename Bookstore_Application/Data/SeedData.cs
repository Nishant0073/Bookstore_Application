using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Application.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seeding Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = "1", Name = "Fiction" },
            new Category { CategoryId = "2", Name = "Science" },
            new Category { CategoryId = "3", Name = "History" }
        );

        // Seeding Books
        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = "1", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 10.99, CategoryId = "1", Stock = 100 },
            new Book { BookId = "2", Title = "The Theory of Everything", Author = "Stephen Hawking", Price = 15.99, CategoryId = "2", Stock = 50 },
            new Book { BookId = "3", Title = "Sapiens: A Brief History of Humankind", Author = "Yuval Noah Harari", Price = 20.99, CategoryId = "3", Stock = 75 }
        );

        // Seeding Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = "1", UserId = "user1", OrderDate = DateTime.Now, TotalPrice = 26.98 },
            new Order { OrderId = "2", UserId = "user2", OrderDate = DateTime.Now.AddDays(-1), TotalPrice = 31.98 }
        );

        // Seeding OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = "1", Quantity = 2, Price = 10.99, BookId = "1", OrderId = "1" },
            new OrderItem { OrderItemId = "2", Quantity = 1, Price = 15.99, BookId = "2", OrderId = "1" },
            new OrderItem { OrderItemId = "3", Quantity = 3, Price = 20.99, BookId = "3", OrderId = "2" }
        ); 
    }
}