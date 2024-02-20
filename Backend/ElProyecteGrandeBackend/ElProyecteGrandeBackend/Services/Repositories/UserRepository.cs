using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Identity;
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
            .Include(user => user.Orders)
            .FirstOrDefault();
    }

    public void DeleteUser(string id)
    {
        using var dbContext = new MarketPlaceContext();
        var user = dbContext.Users.First(user1 => user1.Id == id);
        dbContext.Users.Remove(user);
        dbContext.SaveChanges();
    }

    public void UpdateUser(string id, string name, string email, string phoneNumber)
    {
        using var dbContext = new MarketPlaceContext();
        var userToUpdate = dbContext.Users.Single(user => user.Id == id);
        userToUpdate.UserName = name;
        userToUpdate.NormalizedUserName = name.ToUpper();
        userToUpdate.Email = email;
        userToUpdate.NormalizedEmail = email.ToUpper();
        userToUpdate.PhoneNumber = phoneNumber;
        dbContext.SaveChanges();
        
        // EZT MEGKÉRDEZNI!!!
        /*
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        */
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