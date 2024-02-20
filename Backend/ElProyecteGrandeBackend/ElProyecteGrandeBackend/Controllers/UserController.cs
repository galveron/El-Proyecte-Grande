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
    public async Task<ActionResult<User>> GetUser(string id)
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
    
    [HttpGet("GetUsers")]
    public async Task<ActionResult<User[]>> GetUsers()
    {
        try
        {
            var user = _userRepository.GetAllUsers().ToArray();
            
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
    
    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            _userRepository.DeleteUser(id.ToString());
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    // [HttpPatch("UpdateCustomer")]
    // public async Task<ActionResult> UpdateCustomer(int id, string name, string password, string email, string phoneNumber)
    // {
    //     try
    //     {
    //         _userRepository.UpdateUser(new User
    //         {
    //             Id = id, Name = name, Password = password, Role = Role.Customer, Email = email, PhoneNumber = phoneNumber
    //         });
    //         
    //         return Ok("Successfully added user.");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500);
    //     }
    // }
    
    // [HttpPatch("UpdateCompany")]
    // public async Task<ActionResult> UpdateCompany(int id, string name, string password, string email, string phoneNumber,
    //     string companyName, string identifier)
    // {
    //     try
    //     {
    //         var userFromRepo = _userRepository.GetUser(id);
    //         var company = new Company { Name = companyName, Identifier = identifier, Verified = userFromRepo.Company.Verified };
    //         _userRepository.UpdateUser(new User
    //         {
    //             Id = id, Name = name, Password = password, Role = Role.Customer, Email = email, PhoneNumber = phoneNumber,
    //             Company = company
    //         });
    //         
    //         return Ok("Successfully added user.");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500);
    //     }
    // }
    //
    // [HttpPatch("VerifyCompany")]
    // public async Task<ActionResult> VerifyCompany(int id, bool verified)
    // {
    //     try
    //     {
    //         var companyUser = _userRepository.GetUser(id);
    //         
    //         if (companyUser == null)
    //         {
    //             return NotFound("User was not found");
    //         }
    //         
    //         var company = new Company { Name = companyUser.Company.Name, Identifier = companyUser.Company.Identifier, Verified = verified };
    //         var user = new User
    //         {
    //             Id = companyUser.Id, Name = companyUser.Name, Password = companyUser.Password, Email = companyUser.Email,
    //             PhoneNumber = companyUser.PhoneNumber, Company = company
    //         };
    //         _userRepository.UpdateUser(user);
    //         
    //         return Ok("Successfully added user.");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500);
    //     }
    // }
    //
    [HttpPatch("AddFavourite")]
    public async Task<ActionResult> AddFavourite(string userId, int productId)
    {
        try
        {
            _userRepository.AddFavourite(userId, productId);
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpPatch("AddToCart")]
    public async Task<ActionResult> AddToCart(string userId, int productId)
    {
        try
        {
            _userRepository.AddToCart(userId, productId);
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    //ONLY FOR TEST
    [HttpPost("AddProduct")]
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
                    Seller = _userRepository.GetUser(2.ToString())
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