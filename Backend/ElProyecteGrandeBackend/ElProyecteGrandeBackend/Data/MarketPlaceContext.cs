using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Data;

public class MarketPlaceContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<Order> Orders { get; init; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @$"Server={Environment.GetEnvironmentVariable("SERVER")},{Environment.GetEnvironmentVariable("PORT")};
Database={Environment.GetEnvironmentVariable("DATABASE")};User Id={Environment.GetEnvironmentVariable("USERID")};
Password={Environment.GetEnvironmentVariable("PASSWORD")};Encrypt={Environment.GetEnvironmentVariable("ENCRYPT")};");
    }
}