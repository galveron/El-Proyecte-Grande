using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    IEnumerable<Product> GetAllProductsByUser(User user);
    Product? GetProduct(int productId);
    void AddProduct(Product product);
    void AddMultipleProducts(int userId, IEnumerable<Product> products);
    void DeleteProduct(Product product);
    void UpdateProduct(Product product);
    IEnumerable<Product> GetProductsByPrice(decimal minPrice, decimal maxPrice);
    IEnumerable<Product> GetProductsByDetails(IEnumerable<string> details);
    IEnumerable<Product> GetProductsByQuantity(int quantity);
}