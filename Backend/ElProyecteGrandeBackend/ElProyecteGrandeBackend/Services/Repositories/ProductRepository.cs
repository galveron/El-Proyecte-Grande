using System.Collections.Immutable;
using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<MarketPlaceContext> _optionsBuilder;
    private readonly MarketPlaceContext _dbContext;
    public ProductRepository(MarketPlaceContext marketPlaceContext)
    {
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<MarketPlaceContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _dbContext = marketPlaceContext;
    }
    public IEnumerable<Product> GetAllProducts()
    {
        return _dbContext.Products
            .Include(product => product.Seller)
            .Include(product => product.Images)
            .ToList();
    }
    
    public IEnumerable<Product> GetAllProductsByUser(User user)
    {
        return _dbContext.Products.Where(p => p.Seller.Id == user.Id);
    }

    public Product GetProduct(int productId)
    {
        return _dbContext.Products.FirstOrDefault(p => p.Id == productId);
    }

    public void AddProduct(Product product)
    {
        var userFromDb = _dbContext.Users.FirstOrDefault(user1 => user1.Id == product.Seller.Id);
        
        if (userFromDb == null)
        {
            throw new Exception("User not found for adding product.");
        }
        
        var productFromDb = new Product
            { Name = product.Name, Details = product.Details, Price = product.Price, Quantity = product.Quantity, Seller = userFromDb, Images = product.Images};
        userFromDb.CompanyProducts.Add(productFromDb);
        _dbContext.Update(userFromDb);
        _dbContext.Products.Add(productFromDb);
        _dbContext.SaveChanges();
    }

    public void AddMultipleProducts(int userId, IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _dbContext.Products.Add(product);
        }
        _dbContext.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        _dbContext.Products.Update(product);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Product> GetProductsByPrice(decimal minPrice, decimal maxPrice)
    {
        return _dbContext.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    public IEnumerable<Product> GetProductsByDetails(IEnumerable<string> details)
    {
        var foundProducts = new List<Product>();
        foreach (var detail in details)
        {
            foundProducts.AddRange(_dbContext.Products.Where(p => p.Details.ToLower().Contains(detail.ToLower())));
        }
        return foundProducts;
    }

    public IEnumerable<Product> GetProductsByQuantity(int quantity)
    {
        return _dbContext.Products.Where(p => p.Quantity == quantity);
    }
}
