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
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
        base.OnModelCreating(modelBuilder);
        SeedData.Seed(modelBuilder);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}