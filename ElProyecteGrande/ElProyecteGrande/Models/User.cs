namespace ElProyecteGrandeReact.Models;

public abstract class User
{
    public readonly int Id;
    public readonly string Username;
    private string _password;
    public readonly Role Role;

    public User(string username, string password, Role role)
    {
        Username = username;
        _password = password;
        Role = role;
    }
}