namespace ElProyecteGrandeBackend.Model;

public class Order
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public ICollection<Product> Products { get; init; }
    public decimal PriceToPay { get; init; }

    public Order(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
        Products = new List<Product>();
    }
    
    public Order(int id,int userId, DateTime date, List<Product> products)
    {
        Id = id;
        UserId = userId;
        Date = date;
        Products = products;
        PriceToPay = products.Sum(p => p.Price * p.Quantity);
    }
}