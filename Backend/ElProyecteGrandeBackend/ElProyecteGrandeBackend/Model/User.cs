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
    public List<Product> Favourites { get; init; }
    public List<Product> Cart { get; init; }
}