using ElProyecteGrandeBackend.Data;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Services;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public OrderController(
        IOrderRepository orderRepository, 
        IProductRepository productRepository,
        UserManager<User> userManager)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userManager = userManager;
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

    [HttpGet, Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult<List<Order>>> GetUserOrders()
    {
        try
        {
            var user = await _userManager.Users
                .Include(user => user.Orders)
                .SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);
            var orders = user.Orders;
            
            return Ok(orders);
        }
        catch (Exception e)
        {
            return NotFound("Orders not found");
        }
    }

    [HttpPost, Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult<int>> AddOrder()
    {
        try
        {
            //var user = _userRepository.GetUser(userId);
            var user = await _userManager.Users
                .Include(user1 => user1.Orders)
                .SingleAsync(user1 => user1.UserName == User.Identity.Name);;
            var orderToAdd = new Order {User = user, Date = DateTime.Now, PriceToPay = 0};
            /*user.Orders.Add(orderToAdd);
            
            var identityResult = await _userManager.UpdateAsync(user);
            
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }*/
            
            var orderId = _orderRepository.AddOrder(orderToAdd);
            return Ok(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem("Order failed to add");
        }
    }

    [HttpPost, Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult> AddOrRemoveProductFromOrder(int orderId, int productId, int quantity)
    {
        try
        {
            _orderRepository.AddOrRemoveOrderItems(orderId, productId, quantity);
            
            return Ok("Product added to the order");
        }
        catch (Exception e)
        {
            return Problem("Product failed to add");
        }
    }
    
    [HttpPatch, Authorize(Roles = "Customer, Admin")]
    public async Task<ActionResult> EmptyOrderItems(int orderId)
    {
        try
        {
            _orderRepository.EmptyOrderItems(orderId);
            
            return Ok("Done.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }

    [HttpDelete, Authorize(Roles = "Customer, Admin")]
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
/*
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
    */
}