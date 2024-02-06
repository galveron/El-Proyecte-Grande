namespace ElProyecteGrandeBackend.Model;

public class Product
{
    public int Id { get; init; }
    public int User { get; init; }
    public decimal Price { get; init; }
    public string Details { get; init; }
    public int Quantity { get; init; }
/*
    public Product( decimal price, string details, int quantity)
    {
        Price = price;
        Details = details;
        Quantity = quantity;
    }*/
}