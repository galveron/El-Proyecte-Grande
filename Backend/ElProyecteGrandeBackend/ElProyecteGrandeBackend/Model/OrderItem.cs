using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Model;

[PrimaryKey(nameof(OrderId), nameof(ProductId))]
public class OrderItem
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int Quantity { get; set; }
}