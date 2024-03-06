using ElProyecteGrandeBackend.Model;

namespace ElProyecteGrandeBackend.Services.Repositories;

public interface IOrderRepository
{
    Order GetOrder(int orderId);
    void AddOrder(Order order);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    void AddOrRemoveOrderItems(int orderId, int productId, int quantity);
    void EmptyOrderItems(int orderId);
}