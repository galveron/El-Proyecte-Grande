using System.Security.Claims;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public UserController(
        ILogger<UserController> logger, 
        IUserRepository userRepository, 
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userManager = userManager;
    }

    [HttpGet("GetUser")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        try
        {
            var user = await _userManager.Users
                .Include(user1 => user1.Favourites)
                .Include(user1 => user1.CartItems)
                .Include(user1 => user1.CompanyProducts)
                .Include(user1 => user1.Orders)
                .SingleOrDefaultAsync(user1 => user1.Id == id);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("GetUserFromClaim")]
    public async Task<ActionResult<User>> GetUser()
    {
        try
        {
            var userId = GetIdFromUserClaims();
            var user = await _userManager.Users
                .Include(user1 => user1.Favourites)
                .Include(user1 => user1.CartItems)
                .Include(user1 => user1.CompanyProducts)
                .Include(user1 => user1.Orders)
                .SingleOrDefaultAsync(user1 => user1.Id == userId);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //for testing
    [HttpGet("GetUsers")]
    public async Task<ActionResult<User[]>> GetUsers()
    {
        try
        {
            var users = _userManager.Users.ToArray();
            
            if (users == null)
            {
                return NotFound("Users were not found.");
            }
            
            return Ok(users);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("DeleteUser")]//, Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
            var identityResult = await _userManager.DeleteAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Successfully added user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("UpdateCustomer")]
    public async Task<ActionResult> UpdateCustomer(string id, string userName, string email, string phoneNumber)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = userName;
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            var identityResult = await _userManager.UpdateAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Successfully updated user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("UpdateCompany")]
    public async Task<ActionResult> UpdateCompany(string id, string userName, string email, string phoneNumber,
        string companyName, string identifier)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = userName;
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.Company.Name = companyName;
            user.Company.Identifier = identifier;
            var identityResult = await _userManager.UpdateAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Successfully updated user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("VerifyCompany"), Authorize(Roles="Admin")]
    public async Task<ActionResult> VerifyCompany(string id, bool verified)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Company.Verified = verified;
            var identityResult = await _userManager.UpdateAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Changed verification of company.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("AddFavourite")]
    public async Task<ActionResult> AddFavourite(string userId, int productId)
    {
        try
        {
            var productToAddAsFavourite = _productRepository.GetProduct(productId);
            var userToAddFavouriteTo = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.Id == userId);
        
            //should we test whether the product is already a favourite or would be wasteful since it works anyway??
            //This probably throws an exception
            //because we try to track the Product as productToAddAsFavourite and as part of userToAddFavouriteTo.Favourites.
            
            if (userToAddFavouriteTo == null)
            {
                return NotFound("User was not found for adding favourite.");
            }

            if (productToAddAsFavourite == null)
            {
                return NotFound("Product was not found for adding favourite");
            }
        
            userToAddFavouriteTo.Favourites.Add(productToAddAsFavourite);
            var identityResult = await _userManager.UpdateAsync(userToAddFavouriteTo);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("RemoveFavourite")]
    public async Task<ActionResult> RemoveFavourite(string userId, int productId)
    {
        try
        {
            
            var userToRemoveFavouriteFrom = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.Id == userId);
            
            if (userToRemoveFavouriteFrom == null)
            {
                return NotFound("User was not found for removing favourite.");
            }
            
            var productToRemoveFromFavourite = userToRemoveFavouriteFrom.Favourites.SingleOrDefault(product => product.Id == productId);

            if (productToRemoveFromFavourite == null)
            {
                return NotFound("Product was not found for removing favourite.");
            }
        
            userToRemoveFavouriteFrom.Favourites.Remove(productToRemoveFromFavourite);
            
            Console.WriteLine();
            Console.WriteLine($"productToRemoveFromFavourite.Id: {productToRemoveFromFavourite.Id}");
            Console.WriteLine();
            foreach (var favourite in userToRemoveFavouriteFrom.Favourites)
            {
                Console.WriteLine($"favourite.id: {favourite.Id}");
                Console.WriteLine(productToRemoveFromFavourite == favourite);
            }
            Console.WriteLine();
            
            var identityResult = await _userManager.UpdateAsync(userToRemoveFavouriteFrom);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("AddOrRemoveCartItems")]
    public async Task<ActionResult> AddOrRemoveCartItems(string userId, int productId, int quantity)
    {
        try
        {
            if (quantity == 0)
            {
                return BadRequest("You cannot add or remove zero products.");
            }
            
            var userWhoseCartToAddTo = await _userManager.Users
                .Include(user => user.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .SingleOrDefaultAsync(user => user.Id == userId);
            
            if (userWhoseCartToAddTo == null)
            {
                return NotFound("User was not found for adding to cart.");
            }

            var productToAddToCart = _productRepository.GetProduct(productId); //should we throw errors or return null

            if (productToAddToCart == null)
            {
                return NotFound("Product was not found for adding to cart.");
            }

            var cartItem = userWhoseCartToAddTo.CartItems.SingleOrDefault(cartItem => cartItem.Product.Id == productToAddToCart.Id);
        
            if (cartItem == null)
            {
                if (quantity < 0)
                {
                    return BadRequest("The quantity of products cannot be negative.");
                }
            
                userWhoseCartToAddTo.CartItems.Add(new CartItem
                {
                    CustomerId = userWhoseCartToAddTo.Id,
                    Customer = userWhoseCartToAddTo,
                    ProductId = productToAddToCart.Id,
                    Product = productToAddToCart,
                    Quantity = quantity
                });
            }
            else
            {
                if (cartItem.Quantity + quantity < 0)
                {
                    userWhoseCartToAddTo.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity += quantity;
                }
            }
        
            var identityResult = await _userManager.UpdateAsync(userWhoseCartToAddTo);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("EmptyCart")]
    public async Task<ActionResult> EmptyCart(string userId)
    {
        try
        {
            var userForCartEmptying = await _userManager.Users
                .Include(user => user.CartItems)
                .SingleOrDefaultAsync(user => user.Id == userId);

            if (userForCartEmptying == null)
            {
                return NotFound("User was not found.");
            }
        
            userForCartEmptying.CartItems.Clear();
            var identityResult = await _userManager.UpdateAsync(userForCartEmptying);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    private string GetIdFromUserClaims()
    {
        try
        {
            var userId = User.Claims
                .SkipWhile(claim => claim.Type != ClaimTypes.NameIdentifier)
                .Skip(1)
                .First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Couldn't get user id from User.Claims");
        }
    }
    
    //only for testing
    private void PrintClaims()
    {
        foreach (var userClaim in User.Claims)
        {
            Console.WriteLine($"claim type: {userClaim.Type} claim value: {userClaim.Value}");
        }
    }
}