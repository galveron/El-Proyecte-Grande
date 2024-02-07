namespace ElProyecteGrandeBackend.Model;

public class Order
{
    public int Id { get; init; }
    public User User { get; init; }
    public DateTime Date { get; init; }
    public ICollection<Product> Products { get; } = new List<Product>();
    public decimal PriceToPay { get; set; }
/*
    public Order(User user, DateTime date)
    {
        User = user;
        Date = date;
        Products = new List<Product>();
    }
    
    public Order(User user, DateTime date, ICollection<Product> products)
    {
        User = user;
        Date = date;
        Products = products;
        PriceToPay = products.Sum(p => p.Price * p.Quantity);
    }
*/
    public void AddProduct(Product product)
    {
        Products.Add(product);
        PriceToPay = Products.Sum(p => p.Price * p.Quantity);
    }
}