using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Model;

[PrimaryKey(nameof(CustomerId), nameof(ProductId))]
public class CartItem
{
    public string CustomerId { get; set; }
    public User Customer { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}