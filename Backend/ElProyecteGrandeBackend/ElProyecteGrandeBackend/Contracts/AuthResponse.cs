namespace ElProyecteGrandeBackend.Contracts;

public record AuthResponse(string Email, string UserName,string UserId, string Token);