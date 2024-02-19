using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUser(string id);
    void AddUser(User user);
    void DeleteUser(string id);
    void UpdateUser(User user);
    void AddFavourite(string userId, int productId);
    void AddToCart(User user, Product product);
}