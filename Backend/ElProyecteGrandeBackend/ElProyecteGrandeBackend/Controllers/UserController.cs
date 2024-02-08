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
    
    [HttpGet("AddCustomer")]
    public async Task<ActionResult> AddCustomer(string name, string password, string email, string phoneNumber)
    {
        try
        {
            _userRepository.AddUser(new User
            {
                Name = name, Password = password, Role = Role.Customer, Email = email, PhoneNumber = phoneNumber
            });
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("AddCompanyUser")]
    public async Task<ActionResult> AddCompanyUser(string name, string password, string email, string phoneNumber, 
        string companyName, string identifier)
    {
        try
        {
            var company = new Company { Name = companyName, Identifier = identifier, Verified = false };
            _userRepository.AddUser(new User
            {
                Name = name, Password = password, Role = Role.Company, Email = email, PhoneNumber = phoneNumber,
                Company = company
            });
            
            return Ok("Successfully added company user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("DeleteUser")]
    public async Task<ActionResult> DeleteUser(User user)
    {
        try
        {
            _userRepository.DeleteUser(user);
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("UpdateUser")]
    public async Task<ActionResult> UpdateUser(User user)
    {
        try
        {
            _userRepository.UpdateUser(user);
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("UpdateCompany")]
    public async Task<ActionResult> UpdateCompany(int id, string name, string password, string email, string phoneNumber,
        string companyName, string identifier)
    {
        try
        {
            var userFromRepo = _userRepository.GetUser(id);
            var company = new Company { Name = companyName, Identifier = identifier, Verified = userFromRepo.Company.Verified };
            _userRepository.UpdateUser(new User
            {
                Id = id, Name = name, Password = password, Role = Role.Customer, Email = email, PhoneNumber = phoneNumber,
                Company = company
            });
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpGet("VerifyCompany")]
    public async Task<ActionResult> VerifyCompany(int id, bool verified)
    {
        try
        {
            var companyUser = _userRepository.GetUser(id);
            
            if (companyUser == null)
            {
                return NotFound("User was not found");
            }
            
            var company = new Company { Name = companyUser.Company.Name, Identifier = companyUser.Company.Identifier, Verified = verified };
            var user = new User
            {
                Id = companyUser.Id, Name = companyUser.Name, Password = companyUser.Password, Email = companyUser.Email,
                PhoneNumber = companyUser.PhoneNumber, Company = company
            };
            _userRepository.UpdateUser(user);
            
            return Ok("Successfully added user.");
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
    
    [HttpGet("AddToCart")]
    public async Task<ActionResult> AddToCart(int UserId, int ProductId)
    {
        try
        {
            _userRepository.AddToCart(_userRepository.GetUser(UserId), _productRepository.GetProduct(ProductId));
            
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