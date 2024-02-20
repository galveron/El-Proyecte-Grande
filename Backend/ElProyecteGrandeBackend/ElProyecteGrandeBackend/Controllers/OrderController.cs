using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public OrderController(IUserRepository userRepository, IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<Order> GetOrder(int orderId)
    {
        try
        {
            var order = _orderRepository.GetOrder(orderId);
            return Ok(order);
        }
        catch (Exception e)
        {
            return NotFound("Order not found");
        }
    }

    [HttpGet]
    public ActionResult<List<Order>> GetUserOrders(string userId)
    {
        try
        {
            var orders = _orderRepository.GetUserOrders(userId);
            return Ok(orders);
        }
        catch (Exception e)
        {
            return NotFound("Orders not found");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddOrder(string userId, ICollection<int> productIds)
    {
        try
        {
            var user = _userRepository.GetUser(userId);
            var orderToAdd = new Order {User = user, Date = DateTime.Now, PriceToPay = 0};
            foreach (var productId in productIds)
            {
                var product = _productRepository.GetProduct(productId);
                orderToAdd.Products.Add(product);
                orderToAdd.PriceToPay += product.Price;
            }
            user.Orders.Add(orderToAdd);
            _orderRepository.AddOrder(orderToAdd);
            return Ok("Order added");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem("Order failed to add");
        }
    }

    [HttpDelete]
    public ActionResult DeleteOrder(int orderId)
    {
        try
        {
            var order = _orderRepository.GetOrder(orderId);
            _orderRepository.DeleteOrder(order);
            return Ok("Order successfully deleted");
        }
        catch (Exception e)
        {
            return Problem("Order failed to delete");
        }
    }

    [HttpPatch]
    public ActionResult UpdateOrder(Order order)
    {
        try
        {
            _orderRepository.UpdateOrder(order);
            return Ok("Order successfully updated");
        }
        catch (Exception e)
        {
            return Problem("Order failed to update");
        }
    }
}