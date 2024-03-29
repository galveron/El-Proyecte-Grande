using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Identity;


namespace ElProyecteGrandeBackend.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        var user = new User { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }
        
        await _userManager.AddToRoleAsync(user, role);
        var newUser = _userManager.FindByEmailAsync(email);
        return new AuthResult(true, email, username, newUser.Result.Id,"");
    }
    
    public async Task<AuthResultCompany> RegisterAsyncCompany(string email, string username, string password, string role, string companyName, string identifier)
    {
        var user = new User { UserName = username, Email = email, Company = new Company { Name = companyName, Identifier = identifier, Verified = false}};
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistrationCompany(result, email, username, companyName);
        }

        await _userManager.AddToRoleAsync(user, role);
        return new AuthResultCompany(true, email, username, companyName, "");
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "","");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    private static AuthResultCompany FailedRegistrationCompany(IdentityResult result, string email, string username, string companyName)
    {
        var authResult = new AuthResultCompany(false, email, username, companyName, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }


        // get the role and pass it to the TokenService
        var roles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, roles[0]);

        return new AuthResult(true, managedUser.Email, managedUser.UserName,managedUser.Id, accessToken);
    }

    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "","");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "","");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }
}