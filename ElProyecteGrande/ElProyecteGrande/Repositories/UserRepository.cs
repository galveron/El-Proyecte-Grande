using ElProyecteGrandeReact.Models;

namespace ElProyecteGrandeReact.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = new();
    
    public User GetUser(string username)
    {
        return _users.Single(user => user.Username == username);
    }

    public IEnumerable<User> GetUsers()
    {
        return _users;
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void DeleteUser(string username)
    {
        var itemToRemove = _users.First(user => user.Username == username);
        _users.Remove(itemToRemove);
    }
    
}