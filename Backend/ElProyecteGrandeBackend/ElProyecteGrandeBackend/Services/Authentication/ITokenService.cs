using Microsoft.AspNetCore.Identity;

namespace ElProyecteGrandeBackend.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}