namespace ElProyecteGrandeBackend.Model;

public class Product
{
    public int Id { get; init; }
    public User Seller { get; init; }
    public decimal Price { get; init; }
    public string Details { get; init; }
    public int Quantity { get; init; }
}