using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Data;

public class MarketPlaceContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public MarketPlaceContext()
    {
        
    }
    public MarketPlaceContext (DbContextOptions<MarketPlaceContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @$"Server={Environment.GetEnvironmentVariable("SERVER")},{Environment.GetEnvironmentVariable("PORT")};
Database={Environment.GetEnvironmentVariable("DATABASE")};User Id={Environment.GetEnvironmentVariable("USERID")};
Password={Environment.GetEnvironmentVariable("PASSWORD")};Encrypt={Environment.GetEnvironmentVariable("ENCRYPT")};TrustServerCertificate=true;");
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(e => e.Favourites)
            .WithMany();
        
        builder.Entity<User>()
            .HasMany(e => e.CartItems)
            .WithMany();
        
        builder.Entity<User>()
            .HasMany(e => e.CompanyProducts)
            .WithOne(e => e.Seller)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany();
    }
}