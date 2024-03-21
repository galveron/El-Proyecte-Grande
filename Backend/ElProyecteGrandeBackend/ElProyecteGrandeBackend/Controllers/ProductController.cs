using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public ProductController(
        IOrderRepository orderRepository, 
        IProductRepository productRepository,
        UserManager<User> userManager)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userManager = userManager;
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
    
    [HttpPost("AddProduct"), Authorize(Roles = "Company, Admin")]
    public async Task<ActionResult> AddProduct(string name, decimal price, string details, int quantity)
    {//seller, price, details, quantity
        try
        {
            var user = await _userManager.Users
                .Include(user1 => user1.CompanyProducts)
                .SingleAsync(user1 => user1.UserName == User.Identity.Name);
            var product = new Product{Name = name, Seller = user, Price = price, Details = details, Quantity = quantity};
            _productRepository.AddProduct(product);
            
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound("Product not found");
        }
    }
    
    [HttpDelete("DeleteProduct"), Authorize(Roles = "Company, Admin")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        try
        {
            var product = _productRepository.GetProduct(productId);
            
            if (product == null)
            {
                return NotFound("Product was not found.");
            }
            
            if (product.Seller.UserName != User.Identity.Name)
            {
                return Unauthorized("Not authorized to delete this product");
            }
            
            _productRepository.DeleteProduct(product);
            
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound("Product not found");
        }
    }
}