using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
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
    public ActionResult AddOrder(Order order)
    {
        try
        {
            _orderRepository.AddOrder(order);
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