using ElProyecteGrandeReact.Models;
using ElProyecteGrandeReact.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeReact.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;

    public UserController()
    {
        _userRepository = new UserRepository();
        
    }

    [HttpGet("GetAllUser")]
    public ActionResult<List<User>> GetAll()
    {
        try
        {
            return Ok(_userRepository.GetUsers());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpGet]
    public ActionResult<User> GetUser(string username)
    {
        try
        {
            return Ok(_userRepository.GetUser(username));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult AddAdmin(string username, string password)
    {
        try
        {
            _userRepository.AddUser(new Admin(username, password));
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}