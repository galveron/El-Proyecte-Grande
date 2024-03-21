namespace ElProyecteGrandeBackend.Model;

public class Product
{
    public int Id { get; init; }
    public string Name { get; init; }
    public User Seller { get; init; }
    public decimal Price { get; init; }
    public string Details { get; init; }
    public int Quantity { get; init; }
    public ICollection<Image> Images { get; init; } = new List<Image>();
    public bool Available { get; set; } = true;
}