using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class UserRepository : IUserRepository
{
    public IEnumerable<User> GetAllUsers()
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Users.ToList();
    }

    public User? GetUser(int id)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Users.FirstOrDefault(user => user.Id == id);
    }

    public void AddUser(User user)
    {
        using var dbContext = new MarketPlaceContext();
        var userFromDb = GetUser(user.Id);
        
        if (userFromDb != null)
        {
            throw new Exception("User already in database.");
        }

        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public void DeleteUser(User user)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Users.Remove(user);
        dbContext.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
    }

    public void AddFavourite(User user, Product product)
    {
        using var dbContext = new MarketPlaceContext();
        var userToAddFavourite = GetUser(user.Id);
        var productToAddFavourite = dbContext.Products.FirstOrDefault(product1 => product1.Id == product.Id);
        
        if (userToAddFavourite == null)
        {
            throw new Exception("User was not found for adding favourite.");
        }

        if (productToAddFavourite == null)
        {
            throw new Exception("Product was not found for adding favourite");
        }
        
        userToAddFavourite.Favourites.Add(productToAddFavourite);
        dbContext.SaveChanges();
    }

    public void AddToCart(User user, Product product)
    {
        using var dbContext = new MarketPlaceContext();
        var userToAddToCart = GetUser(user.Id);
        var productToAddToCart = dbContext.Products.FirstOrDefault(product1 => product1.Id == product.Id);
        
        if (userToAddToCart == null)
        {
            throw new Exception("User was not found for adding to cart.");
        }

        if (productToAddToCart == null)
        {
            throw new Exception("Product was not found for adding to cart.");
        }
        
        userToAddToCart.Cart.Add(productToAddToCart);
        dbContext.SaveChanges();
    }
}