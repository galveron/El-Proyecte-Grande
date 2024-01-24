using ElProyecteGrandeReact.Controllers;
using ElProyecteGrandeReact.Models;
using ElProyecteGrandeReact.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ElProyecteGrandeTest;

public class UserControllerTest
{
    private Mock<IUserRepository> _mockUserRepository;
    private UserController _userController;

    [SetUp]
    public void SetUp()
    {
        _userController = new UserController();
    }

    [Test]
    public void GetAllUsersTest()
    {
        var response = _userController.GetAll();
        
        var result = (OkObjectResult)response.Result;
        var resultValue = (IEnumerable<User>)result.Value;
        
        Assert.That(resultValue.Count(), Is.EqualTo(1));
    }
    
}