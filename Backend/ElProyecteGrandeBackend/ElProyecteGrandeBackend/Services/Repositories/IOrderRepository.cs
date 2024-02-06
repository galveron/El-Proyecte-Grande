using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IOrderRepository
{
    Order GetOrder(int orderId);
    List<Order> GetUserOrders(int userId);
    void AddOrder(Order order);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    
}