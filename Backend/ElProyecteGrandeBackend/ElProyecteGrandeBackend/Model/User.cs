namespace ElProyecteGrandeBackend.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Password { get; init; }
    public Role Role { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public Company? Company { get; init; }
    public List<Product> Favourites { get; } = new List<Product>();
    public List<Product> Cart { get; } = new List<Product>();
}