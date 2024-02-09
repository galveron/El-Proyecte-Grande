using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUser(int id);
    void AddUser(User user);
    void DeleteUser(int id);
    void UpdateUser(User user);
    void AddFavourite(int userId, int productId);
    void AddToCart(User user, Product product);
}