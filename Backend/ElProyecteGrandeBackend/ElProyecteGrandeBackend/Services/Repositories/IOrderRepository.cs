using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IOrderRepository
{
    Order GetOrder(int orderId);
    List<Order> GetUserOrders(int userId);
    void AddOrder(int userId, int productId);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    
}