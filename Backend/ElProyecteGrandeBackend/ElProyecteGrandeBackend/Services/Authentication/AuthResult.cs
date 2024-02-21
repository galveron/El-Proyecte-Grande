namespace ElProyecteGrandeBackend.Services.Authentication;

public record AuthResult(
    bool Success,
    string Email,
    string UserName,
    string UserId,
    string Token)
{
    //Error code - error message
    public readonly Dictionary<string, string> ErrorMessages = new();
}