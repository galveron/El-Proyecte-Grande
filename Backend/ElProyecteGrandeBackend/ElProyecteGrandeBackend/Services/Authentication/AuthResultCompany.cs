namespace ElProyecteGrandeBackend.Services.Authentication;

public record AuthResultCompany(
    bool Success,
    string Email,
    string UserName,
    string CompanyName,
    string Token)
{
    //Error code - error message
    public readonly Dictionary<string, string> ErrorMessages = new();
}