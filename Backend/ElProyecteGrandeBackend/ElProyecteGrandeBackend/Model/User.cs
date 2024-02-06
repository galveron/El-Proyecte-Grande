using System.ComponentModel.DataAnnotations.Schema;

namespace ElProyecteGrandeBackend.Model;

[Table("users")]
public class User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Password { get; init; }
    public Role Role { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public Company? Company { get; init; }
    public ICollection<Product> Favourites { get; } = new List<Product>();
    public ICollection<Product> CartItems { get; } = new List<Product>();
    public ICollection<Product> CompanyProducts { get; } = new List<Product>();
}