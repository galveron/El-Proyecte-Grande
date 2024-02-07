using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    
    public Order GetOrder(int orderId)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
    }

    public List<Order> GetUserOrders(int userId)
    {
        using var dbContext = new MarketPlaceContext();
        return null;
    }

    public void AddOrder(int userId, int productId)
    {
        var user = new UserRepository().GetUser(userId);
        var product = new ProductRepository().GetProduct(productId);
        using var dbContext = new MarketPlaceContext();
        var orderToAdd = new Order {User = user, Date = DateTime.Now, PriceToPay = product.Price};
        orderToAdd.Products.Add(product);
        dbContext.Update(orderToAdd);
        dbContext.Orders.Add(orderToAdd);
        user.Orders.Add(orderToAdd);
        dbContext.Update(user);
        dbContext.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Orders.Remove(order);
        dbContext.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Orders.Update(order);
        dbContext.SaveChanges();
    }
}