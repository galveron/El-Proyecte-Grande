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
    private readonly IJsonProcessor _jsonProcessor;

    public OrderController(IOrderRepository orderRepository, IJsonProcessor jsonProcessor)
    {
        _jsonProcessor = jsonProcessor;
        _orderRepository = orderRepository;
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
    public ActionResult<List<Order>> GetUserOrders(int userId)
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
    public ActionResult AddOrder(int userId )
    {
        try
        {
            _orderRepository.AddOrder(new Order(userId, DateTime.Now));
            return Ok("Order added");
        }
        catch (Exception e)
        {
            return Problem("Order failed to add");
        }
    }

    [HttpDelete]
    public ActionResult DeleteOrder(Order order)
    {
        try
        {
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