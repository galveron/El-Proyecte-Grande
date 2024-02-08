using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public ProductController(IUserRepository userRepository, IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    [HttpGet("GetAllProducts")]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            var products = _productRepository.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return NotFound("Product not found");
        }
    }

    [HttpGet("GetProduct")]
    public ActionResult<Product> GetProduct(int id)
    {
        try
        {
            var product = _productRepository.GetProduct(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound("Product not found");
        }
    }
    
    [HttpGet("AddProduct")]
    public ActionResult AddProduct(int userId, decimal price, string details, int quantity)
    {//seller, price, details, quantity
        try
        {
            var user = _userRepository.GetUser(userId);
            var product = new Product{Seller = user, Price = price, Details = details, Quantity = quantity};
            _productRepository.AddProduct(product);
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound("Product not found");
        }
    }
    

}