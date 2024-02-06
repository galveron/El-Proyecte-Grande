using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private IUserRepository _userRepository;
    private IOrderRepository _orderRepository;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }
}