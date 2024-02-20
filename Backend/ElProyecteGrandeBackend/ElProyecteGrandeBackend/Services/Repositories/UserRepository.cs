using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class UserRepository : IUserRepository
{
    public IEnumerable<User> GetAllUsers()
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Users.ToList();
    }

    public User? GetUser(string id)
    {
        using var dbContext = new MarketPlaceContext();
        
        return dbContext.Users.Where(user => user.Id == id)
            .Include(user => user.Favourites)
            .Include(user => user.CompanyProducts)
            .Include(user => user.CartItems)
            .FirstOrDefault();
    }

    public void DeleteUser(string id)
    {
        using var dbContext = new MarketPlaceContext();
        var user = dbContext.Users.First(user1 => user1.Id == id);
        dbContext.Users.Remove(user);
        dbContext.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
    }

    public void AddFavourite(string userId, int productId)
    {
        using var dbContext = new MarketPlaceContext();
        var productToAddFavourite = dbContext.Products.Find(productId);
        var userToAddFavourite = dbContext.Users.Include(user => user.Favourites)
            .SingleOrDefault(user => user.Id == userId);
        
        if (userToAddFavourite == null)
        {
            throw new Exception("User was not found for adding favourite.");
        }

        if (productToAddFavourite == null)
        {
            throw new Exception("Product was not found for adding favourite");
        }
        
        userToAddFavourite.Favourites.Add(productToAddFavourite);
        dbContext.Update(userToAddFavourite);
        dbContext.SaveChanges();
    }

    public void AddToCart(string userId, int productId)
    {
        using var dbContext = new MarketPlaceContext();
        var userToAddToCart = GetUser(userId);
        var productToAddToCart = dbContext.Products.FirstOrDefault(product1 => product1.Id == productId);
        
        if (userToAddToCart == null)
        {
            throw new Exception("User was not found for adding to cart.");
        }

        if (productToAddToCart == null)
        {
            throw new Exception("Product was not found for adding to cart.");
        }
        
        userToAddToCart.CartItems.Add(productToAddToCart);
        dbContext.Update(userToAddToCart);
        dbContext.SaveChanges();
    }
    
}