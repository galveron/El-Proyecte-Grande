using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Data;

public class MarketPlaceContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @$"Server={Environment.GetEnvironmentVariable("SERVER")},{Environment.GetEnvironmentVariable("PORT")};
Database={Environment.GetEnvironmentVariable("DATABASE")};User Id={Environment.GetEnvironmentVariable("USERID")};
Password={Environment.GetEnvironmentVariable("PASSWORD")};Encrypt={Environment.GetEnvironmentVariable("ENCRYPT")};");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();
        builder.Entity<Company>(company => company.HasNoKey());
        builder.Entity<User>().Ignore("Company");
        builder.Entity<Product>()
            .HasIndex(p => p.Id)
            .IsUnique();
        builder.Entity<Product>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey("User");
        builder.Entity<Order>()
            .HasIndex(o => o.Id)
            .IsUnique();
    }
}