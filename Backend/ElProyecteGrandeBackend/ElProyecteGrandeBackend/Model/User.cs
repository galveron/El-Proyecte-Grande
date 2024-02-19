using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ElProyecteGrandeBackend.Model;

[Table("users")]
public class User : IdentityUser
{
    public Company? Company { get; init; }
    public ICollection<Product> Favourites { get; } = new List<Product>();
    public ICollection<Product> CartItems { get; } = new List<Product>();
    public ICollection<Product> CompanyProducts { get; } = new List<Product>();
    public ICollection<Order> Orders { get; } = new List<Order>();
}