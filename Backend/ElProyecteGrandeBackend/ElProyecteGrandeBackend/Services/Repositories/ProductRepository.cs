using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class ProductRepository : IProductRepository
{
    public IEnumerable<Product> GetAllProducts()
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Products;
    }
    
    public IEnumerable<Product> GetAllProductsByUser(int userId)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Products.Where(p => p.UserId == userId);
    }

    public Product GetProduct(int productId)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Products.FirstOrDefault(p => p.Id == productId);
    }

    public void AddProduct(Product product)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
    }

    public void AddMultipleProducts(int userId, IEnumerable<Product> products)
    {
        using var dbContext = new MarketPlaceContext();
        foreach (var product in products)
        {
            dbContext.Products.Add(product);
        }
        dbContext.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Products.Remove(product);
        dbContext.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Products.Update(product);
        dbContext.SaveChanges();
    }

    public IEnumerable<Product> GetProductsByPrice(decimal minPrice, decimal maxPrice)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    public IEnumerable<Product> GetProductsByDetails(IEnumerable<string> details)
    {
        using var dbContext = new MarketPlaceContext();
        var foundProducts = new List<Product>();
        foreach (var detail in details)
        {
            foundProducts.AddRange(dbContext.Products.Where(p => p.Details.ToLower().Contains(detail.ToLower())));
        }
        return foundProducts;
    }

    public IEnumerable<Product> GetProductsByQuantity(int quantity)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Products.Where(p => p.Quantity == quantity);
    }
}
