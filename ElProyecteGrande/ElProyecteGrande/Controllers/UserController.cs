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
    public ActionResult<List<User>> GetAll()
    {
        try
        {
            return Ok(_userRepository.GetUsers().ToList());
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