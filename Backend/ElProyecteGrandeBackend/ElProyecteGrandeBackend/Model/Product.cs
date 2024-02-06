namespace ElProyecteGrandeBackend.Model;

public class Product
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public decimal Price { get; init; }
    public string Details { get; init; }
    public int Quantity { get; init; }

    public Product(int userId, decimal price, string details, int quantity)
    {
        UserId = userId;
        Price = price;
        Details = details;
        Quantity = quantity;
    }
}