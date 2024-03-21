using System.Collections.ObjectModel;
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
    private readonly IImageRepository _imageRepository;

    public ProductController(
        IOrderRepository orderRepository, 
        IProductRepository productRepository,
        UserManager<User> userManager, IImageRepository imageRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userManager = userManager;
        _imageRepository = imageRepository;
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
    public async Task<ActionResult> AddProduct(ICollection<IFormFile> images, string name, decimal price, string details, int quantity)
    {
        var imagesCollection = new Collection<Image>();
        
        if (images != null)
        {
            foreach (var image in images)
            {
                var key = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var result = await _imageRepository.UploadObjectAsyncToAWS(key, image);
                imagesCollection.Add(new Image{ImageURL = result});
                if (result == null)
                {
                    return Problem("Image upload is not successful");
                };
            }
        }
        
        try
        {
            var user = await _userManager.Users
                .Include(user1 => user1.CompanyProducts)
                .SingleAsync(user1 => user1.UserName == User.Identity.Name);
            
            var product = new Product{Name = name, Seller = user, Price = price, Details = details, Quantity = quantity, Images = imagesCollection};
            
            _productRepository.AddProduct(product);
            
            return Ok(product);
        }
        catch (Exception e)
        {
            return NotFound("Adding product is failed");
        }
    }
    
    [HttpDelete("DeleteProduct")]
    public ActionResult<Product> DeleteProduct(int id)
    {
        try
        {
            var product = _productRepository.GetProduct(id);
            Console.WriteLine("product: " + product.Name);
            _productRepository.DeleteProduct(product);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound("Product delete not successful");
        }
    }
}