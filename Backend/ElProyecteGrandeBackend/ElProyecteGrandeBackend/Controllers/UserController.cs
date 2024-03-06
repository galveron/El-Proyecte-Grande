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
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public UserController(
        ILogger<UserController> logger,
        IProductRepository productRepository,
        UserManager<User> userManager)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userManager = userManager;
    }

    [HttpGet("GetUser")]
    public async Task<ActionResult<User>> GetUser(string userName)
    {
        try
        {
            var user = await _userManager.Users
                .Include(user1 => user1.Favourites)
                .Include(user1 => user1.CartItems)
                .Include(user1 => user1.CompanyProducts)
                .Include(user1 => user1.Orders)
                .SingleOrDefaultAsync(user1 => user1.UserName == userName);
            
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
            
            if (users.Length == 0)
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
    public async Task<ActionResult> DeleteUser(string userName)
    {
        //TODO: companies can't be deleted yet (server error)
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == userName);
            
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
    public async Task<ActionResult> UpdateCustomer(string oldUserName, string newUserName, string email, string phoneNumber)
    {
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == oldUserName);
            
            if (user == null)
            {
                return BadRequest("User was not found");
            }

            if (oldUserName == newUserName)
            {
                return BadRequest("New username cannot be the same as old username.");
            }
            
            user.UserName = newUserName;
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
    public async Task<ActionResult> UpdateCompany(string oldUserName, string newUserName, string email, string phoneNumber,
        string companyName, string identifier)
    {
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == oldUserName);

            if (user == null)
            {
                return BadRequest("User was not found");
            }
            
            user.UserName = newUserName;
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
    
    [HttpPatch("VerifyCompany")]//,Authorize(Roles="Admin")]
    public async Task<ActionResult> VerifyCompany(string userName, bool verified)
    {
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == userName);

            if (user == null)
            {
                return BadRequest("User was not found");
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
    
    [HttpPatch("AddFavourite")]
    public async Task<ActionResult> AddFavourite(string userName, int productId)
    {
        try
        {
            var productToAddAsFavourite = _productRepository.GetProduct(productId);
            var userToAddFavouriteTo = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.UserName == userName);
        
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
    public async Task<ActionResult> RemoveFavourite(string userName, int productId)
    {
        try
        {
            
            var userToRemoveFavouriteFrom = await _userManager.Users
                .Include(user => user.Favourites)
                .SingleOrDefaultAsync(user => user.UserName == userName);
            
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
    
    [HttpPatch("AddOrRemoveCartItems")]//, Authorize(Roles="Customer, Admin")]
    public async Task<ActionResult> AddOrRemoveCartItems(string userName, int productId, int quantity)
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
                .SingleOrDefaultAsync(user => user.UserName == userName);
            
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
    public async Task<ActionResult> EmptyCart(string userName)
    {
        try
        {
            var userForCartEmptying = await _userManager.Users
                .Include(user => user.CartItems)
                .SingleOrDefaultAsync(user => user.UserName == userName);

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