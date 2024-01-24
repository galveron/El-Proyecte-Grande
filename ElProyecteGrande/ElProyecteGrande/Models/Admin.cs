namespace ElProyecteGrandeReact.Models;

public class Admin : User
{
    public Admin(int id, string username, string password) : base(id, username, password, Role.Admin)
    {
        
    }
}