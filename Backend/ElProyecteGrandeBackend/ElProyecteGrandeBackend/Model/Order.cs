namespace ElProyecteGrandeBackend.Model;

public class Order
{
    public int Id { get; init; }
    public User User { get; init; }
    public DateTime Date { get; init; }
    public ICollection<Product> Products { get; } = new List<Product>();
    public decimal PriceToPay { get; set; }
    
}