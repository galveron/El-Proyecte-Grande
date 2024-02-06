namespace ElProyecteGrandeBackend.Model;

public class Order
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public List<Product> Products { get; init; }
    public decimal PriceToPay { get; init; }

    public Order(int userId, DateTime date, List<Product> products)
    {
        UserId = userId;
        Date = date;
        Products = products;
        PriceToPay = products.Sum(p => p.Quantity * p.Price);
    }
}