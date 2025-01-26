using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;
namespace Bookstore_Application.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //To allow only unique values for categories name
        modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
        modelBuilder.Entity<Order>().HasMany(o => o.OrderItems).WithOne(o => o.Order).HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
        SeedData.Seed(modelBuilder);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}