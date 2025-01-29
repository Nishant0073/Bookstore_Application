using Bookstore_Application.Constants;
using Bookstore_Application.Models;
using Bookstore_Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Application.Data;

public class SeedData
{

    public SeedData()
    {
    }
    public static void Seed(ModelBuilder modelBuilder)
    {
        
        // Seeding Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = "CK1", CategoryName = "Fiction" },
            new Category { CategoryId = "CK2", CategoryName = "Science" },
            new Category { CategoryId = "CK3", CategoryName = "History" }
        );

        // Seeding Books
        
        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = "BK1", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 10.99, CategoryId = "CK1", Stock = 100 },
            new Book { BookId = "BK2", Title = "The Theory of Everything", Author = "Stephen Hawking", Price = 15.99, CategoryId = "CK2", Stock = 50 },
            new Book { BookId = "BK3", Title = "Sapiens: A Brief History of Humankind", Author = "Yuval Noah Harari", Price = 20.99, CategoryId = "CK3", Stock = 75 }
        );

        // Seeding Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = "OK1", UserId = "user1", OrderDate = DateTime.Now, TotalPrice = 26.98 },
            new Order { OrderId = "OK2", UserId = "user2", OrderDate = DateTime.Now.AddDays(-1), TotalPrice = 31.98 }
        );

        // Seeding OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = "OIK1", Quantity = 2, Price = 10.99, BookId = "BK1", OrderId = "OK1" },
            new OrderItem { OrderItemId = "OIK2", Quantity = 1, Price = 15.99, BookId = "BK2", OrderId = "OK1" },
            new OrderItem { OrderItemId = "OIK3", Quantity = 3, Price = 20.99, BookId = "BK3", OrderId = "OK2" }
        ); 
    }

    public static async Task SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //seeding roles
        await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.User.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Admin.ToString()));
        
        //seed user
        var defaultUser = new IdentityUser { UserName = Authorization._defaultEmail, Email = Authorization._defaultEmail ,EmailConfirmed = true };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            await userManager.CreateAsync(defaultUser, Authorization._defaultPassword);
            await userManager.AddToRoleAsync(defaultUser, Authorization._defaultRole.ToString());
        }
    }
}