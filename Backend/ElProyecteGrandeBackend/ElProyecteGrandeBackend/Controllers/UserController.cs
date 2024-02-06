using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public UserController(
        ILogger<UserController> logger, 
        IUserRepository userRepository, 
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    [HttpGet("GetUser")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var user = _userRepository.GetUser(id);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("AddFavourite")]
    public async Task<ActionResult> AddFavourite(int UserId, int ProductId)
    {
        try
        {
            _userRepository.AddFavourite(_userRepository.GetUser(UserId), _productRepository.GetProduct(ProductId));
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    //ONLY FOR TEST
    [HttpGet("AddProduct")]
    public async Task<ActionResult> AddProduct()
    {
        try
        {
            _productRepository.AddProduct(
                new Product
                {
                    Details = "Hát persze, hogy minőségi",
                    Price = 999,
                    Quantity = 666,
                    Seller = _userRepository.GetUser(2)
                }
                );
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
}