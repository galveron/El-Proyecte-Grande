using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUser(string id);
    void DeleteUser(string id);
    void UpdateUser(string id, string name, string email, string phoneNumber);
    void AddFavourite(string userId, int productId);
    void AddToCart(string userId, int productId);
}