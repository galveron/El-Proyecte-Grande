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
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public UserController(
        ILogger<UserController> logger, 
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        UserManager<User> userManager)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userManager = userManager;
    }

    [HttpGet("GetUser"), Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult<User>> GetUser()
    {
        try
        {
            var user = await _userManager.Users
                .Include(user1 => user1.Favourites)
                .Include(user1 => user1.CartItems)
                .Include(user1 => user1.CompanyProducts)
                .Include(user1 => user1.Orders)
                .SingleOrDefaultAsync(user1 => user1.UserName == User.Identity.Name);
            
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
    
    [HttpDelete("DeleteUserForAdmin"), Authorize(Roles = "Admin")]
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
            
            return Ok("Successfully deleted user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("DeleteUser"), Authorize(Roles = "Customer, Company")]
    public async Task<ActionResult> DeleteUser()
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
            var identityResult = await _userManager.DeleteAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Successfully deleted user.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("UpdateCustomer"), Authorize(Roles = "Customer, Company, Admin")]
    public async Task<ActionResult> UpdateCustomer(string userName, string email, string phoneNumber)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
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
    
    [HttpPatch("UpdateCompany"), Authorize(Roles = "Company, Admin")]
    public async Task<ActionResult> UpdateCompany(string userName, string email, string phoneNumber,
        string companyName, string identifier)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
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
            
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            
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
    
    [HttpPatch("AddFavourite"), Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult> AddFavourite(int productId)
    {
        try
        {
            var userToAddFavouriteTo = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);

            if (userToAddFavouriteTo == null)
            {
                return NotFound("User was not found for adding favourite.");
            }
            
            if (userToAddFavouriteTo.Favourites.SingleOrDefault(product => product.Id == productId) != null)
            {
                return Ok("Product is already a favourite.");
            }
            
            var productToAddAsFavourite = _productRepository.GetProduct(productId);
            
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

            return Ok("Successfully added product to favourites.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("RemoveFavourite"), Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult> RemoveFavourite(int productId)
    {
        try
        {
            
            var userToRemoveFavouriteFrom = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);
            
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
            
            var identityResult = await _userManager.UpdateAsync(userToRemoveFavouriteFrom);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }
            
            return Ok("Successfully removed product to favourites.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("AddOrRemoveCartItems"), Authorize(Roles="Customer, Admin")]
    public async Task<ActionResult> AddOrRemoveCartItems(int productId, int quantity)
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
                .SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);
            
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
            
            return Ok("Successfully added product to cart.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPatch("EmptyCart"), Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult> EmptyCart()
    {
        try
        {
            var userForCartEmptying = await _userManager.Users
                .Include(user => user.CartItems)
                .SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);

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
}