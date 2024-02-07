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

    /*
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
        builder.Entity<Product>();
        builder.Entity<Order>()
            .HasIndex(o => o.Id)
            .IsUnique();
    }*/
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasMany(e => e.Favourites)
            .WithMany();
        
        builder.Entity<User>()
            .HasMany(e => e.CartItems)
            .WithMany();
        
        /*
        builder.Entity<User>()
            .HasMany(e => e.CompanyProducts)
            .WithMany();*/
        
        builder.Entity<User>()
            .HasMany(e => e.CompanyProducts)
            .WithOne(e => e.Seller)
            //.HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .OnDelete(DeleteBehavior.NoAction);
        /*
        builder.Entity<Order>()
            .HasIndex(o => o.Id)
            .IsUnique();
        
        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany();*/
        builder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany();

        /*
        builder.Entity<Company>()
            .HasData(
                new Company
                {
                    Name = "Bélának Jó Lesz Cég", Identifier = "01-10-666999", Verified = true
                }
            );*/
        
        builder.Entity<User>()
            .HasData(
                new User
                {
                    Id = 1, Email = "bela@gmail.com", Name = "BÉÉLA", Password = "BEEEEELAAA", PhoneNumber = "BEL-0000",
                    Role = Role.Customer
                },
                new User
                {
                    Id = 2, Email = "forbela@gmail.com", Name = "Béla barát", Password = "lehúzzukbélátDENAGYON", PhoneNumber = "NEMBEL-666",
                    Role = Role.Company
                }
            );

        builder.Entity<User>().OwnsOne(p => p.Company)
            .HasData(
                new {UserId = 2, Name = "Bélának Jó Lesz Cég", Identifier = "01-10-666999", Verified = true}
            );
        
    }
}