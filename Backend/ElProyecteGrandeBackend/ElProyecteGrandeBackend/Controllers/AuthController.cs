using Azure.Core;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Services;
using ElProyecteGrandeBackend.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;

    public AuthController(IAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        return await RegisterWithRole(request, "Customer");
    }
    
    [HttpPost("RegisterCompany")]
    public async Task<ActionResult<RegistrationResponse>> RegisterCompany(RegistrationRequest request)
    {
        return await RegisterWithRole(request, "Company");
    }

    private async Task<ActionResult<RegistrationResponse>> RegisterWithRole(RegistrationRequest request, string role)
    {
        //if admin throw error
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password, role);
        Console.WriteLine(result);
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
    }
}