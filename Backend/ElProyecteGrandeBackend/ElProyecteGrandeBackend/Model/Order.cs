namespace ElProyecteGrandeBackend.Model;

public class Order
{
    public int Id { get; init; }
    public User User { get; init; }
    public DateTime Date { get; init; }
    public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
    public decimal PriceToPay { get; set; }
    
}