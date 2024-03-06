using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUser(string id);
    void DeleteUser(string id);
    void UpdateUser(User user);
    void AddFavourite(string userId, int productId);
    public void DeleteFavourite(string userId, int productId);
    void AddToCart(string userId, int productId, int quantity);
    void EmptyCart(string userId);
}