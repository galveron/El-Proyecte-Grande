using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Services;

namespace ElProyecteGrandeIntegrationTest;

public class AuthControllerTest
{
    [Fact]
    public async Task UserRegistrationTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequest("valaki@g.com", "valaki", "valaki");

        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync("/Auth/Register", request);

        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Console.WriteLine(authResponse?.UserId);
    }
}