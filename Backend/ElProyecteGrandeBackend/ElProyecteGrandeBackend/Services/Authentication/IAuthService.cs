namespace ElProyecteGrandeBackend.Services.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role);

    Task<AuthResultCompany> RegisterAsyncCompany(string email, string username, string password, string role, string companyName, string identifier);
    Task<AuthResult> LoginAsync(string email, string password);
}