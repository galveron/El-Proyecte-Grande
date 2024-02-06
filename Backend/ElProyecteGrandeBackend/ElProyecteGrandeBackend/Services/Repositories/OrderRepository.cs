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
        var orders = dbContext.Orders.Where(order => order.UserId == userId).ToList();
        return orders;
    }

    public void AddOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Orders.Add(order);
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