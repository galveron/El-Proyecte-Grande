using Azure.Core;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Services;
using ElProyecteGrandeBackend.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<RegistrationResponse>> RegisterCustomer(RegistrationRequest request)
    {
        var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password, "Customer");
        Console.WriteLine(result);
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(RegisterCustomer), new RegistrationResponse(result.Email, result.UserName));

    }
    
    [HttpPost("RegisterCompany")]
    public async Task<ActionResult<RegistrationResponseCompany>> RegisterCompany(RegistrationRequestCompany request)
    {
        var result = await _authenticationService.RegisterAsyncCompany(request.Email, request.Username, request.Password, "Company", request.CompanyName, request.Identifier);
        Console.WriteLine(result);
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(RegisterCustomer), new RegistrationResponseCompany(result.Email, result.UserName, result.CompanyName));

    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    private void AddErrors(AuthResultCompany result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult> Authenticate([FromBody] AuthRequest request)
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
        
        Response.Cookies.Append("User", result.Token, new CookieOptions() { HttpOnly = false, SameSite = SameSiteMode.Strict });

        return Ok();
    }
    
    [Authorize(Roles = "Customer, Company, Admin")]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("User");
        return Ok();
    }
}