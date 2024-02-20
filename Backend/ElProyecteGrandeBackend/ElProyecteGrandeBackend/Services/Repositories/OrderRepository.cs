using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace ElProyecteGrandeBackend.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IUserRepository _userRepository;

    public OrderRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Order GetOrder(int orderId)
    {
        using var dbContext = new MarketPlaceContext();
        return dbContext.Orders.Where(o => o.Id == orderId)
            .Include(order => order.User)
            .Include(order => order.Products)
            .First();
    }

    public List<Order> GetUserOrders(string userId)
    {
        using var dbContext = new MarketPlaceContext();
        var user = _userRepository.GetUser(userId);
        return user.Orders.ToList();
    }

    public void AddOrder(Order order)
    {
        using var dbContext = new MarketPlaceContext();
        dbContext.Update(order);
        dbContext.Orders.Add(order);
        dbContext.Update(order.User);
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