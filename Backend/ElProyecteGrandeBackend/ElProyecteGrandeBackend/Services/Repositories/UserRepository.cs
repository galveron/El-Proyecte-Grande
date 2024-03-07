using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<MarketPlaceContext> _optionsBuilder;
    private readonly MarketPlaceContext _dbContext;

    public UserRepository(MarketPlaceContext marketPlaceContext)
    {
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<MarketPlaceContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _dbContext = marketPlaceContext;
    }
    public IEnumerable<User> GetAllUsers()
    {
        return _dbContext.Users.ToList();
    }

    public User? GetUser(string id)
    {
        return _dbContext.Users.Where(user => user.Id == id)
            .Include(user => user.Favourites)
            .Include(user => user.CompanyProducts)
            .Include(user => user.CartItems)
            .Include(user => user.Orders)
            .FirstOrDefault();
    }

    public void DeleteUser(string id)
    {
        var user = _dbContext.Users.First(user1 => user1.Id == id);
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        /*
        using var dbContext = new MarketPlaceContext();
        var userToUpdate = dbContext.Users.Single(user => user.Id == id);
        userToUpdate.UserName = name;
        userToUpdate.NormalizedUserName = name.ToUpper();
        userToUpdate.Email = email;
        userToUpdate.NormalizedEmail = email.ToUpper();
        userToUpdate.PhoneNumber = phoneNumber;
        */
        _dbContext.Update(user);
        _dbContext.SaveChanges();
        
        // EZT MEGKÉRDEZNI!!!
        /*
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        */
    }

    public void AddFavourite(string userId, int productId)
    {
        var productToAddFavourite = _dbContext.Products.Find(productId);
        var userToAddFavourite = _dbContext.Users.Include(user => user.Favourites)
            .SingleOrDefault(user => user.Id == userId);
        
        if (userToAddFavourite == null)
        {
            throw new Exception("User was not found for adding favourite.");
        }

        if (productToAddFavourite == null)
        {
            throw new Exception("Product was not found for adding favourite");
        }
        
        //should we test whether the product is already a favourite or would be wasteful since it works anyway??
        
        userToAddFavourite.Favourites.Add(productToAddFavourite);
        _dbContext.Update(userToAddFavourite);
        _dbContext.SaveChanges();
    }
    
    public void DeleteFavourite(string userId, int productId)
    {
        var productToRemoveFromFavourite = _dbContext.Products.Find(productId);
        var userToRemoveFavouriteFrom = _dbContext.Users.Include(user => user.Favourites)
            .SingleOrDefault(user => user.Id == userId);
        
        if (userToRemoveFavouriteFrom == null)
        {
            throw new Exception("User was not found for removing favourite.");
        }

        if (productToRemoveFromFavourite == null)
        {
            throw new Exception("Product was not found for removing favourite");
        }
        
        //should we test whether the product is already a favourite or would be wasteful since it works anyway??
        
        userToRemoveFavouriteFrom.Favourites.Remove(productToRemoveFromFavourite);
        _dbContext.Update(userToRemoveFavouriteFrom);
        _dbContext.SaveChanges();
    }
    
    public void AddToCart(string userId, int productId, int quantity)
    {
        if (quantity == 0)
        {
            throw new Exception("You cannot add zero product.");
        }
        
        var userToAddToCart = _dbContext.Users
            .Include(user => user.CartItems)
            .ThenInclude(cartItem => cartItem.Product)
            .SingleOrDefault(user => user.Id == userId);
        var productToAddToCart = _dbContext.Products.FirstOrDefault(product1 => product1.Id == productId);
        
        if (userToAddToCart == null)
        {
            throw new Exception("User was not found for adding to cart.");
        }

        if (productToAddToCart == null)
        {
            throw new Exception("Product was not found for adding to cart.");
        }

        var cartItem = userToAddToCart.CartItems.SingleOrDefault(cartItem => cartItem.Product.Id == productToAddToCart.Id);
        
        if (cartItem == null)
        {
            if (quantity < 0)
            {
                throw new Exception("The quantity of products cannot be negative.");
            }
            
            userToAddToCart.CartItems.Add(new CartItem
            {
                CustomerId = userToAddToCart.Id,
                Customer = userToAddToCart,
                ProductId = productToAddToCart.Id,
                Product = productToAddToCart,
                Quantity = quantity
            });
        }
        else
        {
            if (cartItem.Quantity + quantity < 0)
            {
                userToAddToCart.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
        }
        
        _dbContext.Update(userToAddToCart);
        _dbContext.SaveChanges();
    }

    public void EmptyCart(string userId)
    {
        var userForCartEmptying = _dbContext.Users
            .Include(user => user.CartItems)
            .Single(user => user.Id == userId);
        
        userForCartEmptying.CartItems.Clear();
        
        _dbContext.SaveChanges();
    }
}