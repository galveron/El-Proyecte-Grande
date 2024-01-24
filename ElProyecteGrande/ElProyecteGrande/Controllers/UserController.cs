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
        _userRepository.AddUser(new Admin(1,"bence", "bence"));
    }

    [HttpGet("GetAllUser")]
    public List<User> GetAll()
    {
        try
        {
            var users = _userRepository.GetUsers().ToList();
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
    public IActionResult AddAdmin(int id,string username, string password)
    {
        try
        {
            _userRepository.AddUser(new Admin(id,username, password));
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}