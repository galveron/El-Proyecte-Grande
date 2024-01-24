using ElProyecteGrandeReact.Models;

namespace ElProyecteGrandeReact.Repositories;

public interface IUserRepository
{
    User GetUser(string username);
    IEnumerable<User> GetUsers();
    void AddUser(User user);
    void DeleteUser(string username);
}