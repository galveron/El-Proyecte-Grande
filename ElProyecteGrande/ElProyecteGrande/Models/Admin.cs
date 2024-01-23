namespace ElProyecteGrandeReact.Models;

public class Admin : User
{
    public Admin(string username, string password) : base(username, password, Role.Admin)
    {
        
    }
}