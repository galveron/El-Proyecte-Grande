using System.Net;
using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Services;

namespace ElProyecteGrandeIntegrationTest;

[Collection("Integration")]
public class AuthControllerTest
{
    [Fact]
    public async Task UserRegistrationSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequest("valaki@g.com", "valaki", "Valaki123456");

        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync("/Auth/Register", request);
        
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        
        Assert.Equal("valaki", authResponse.UserName);
    }

    [Fact]
    public async Task UserRegistrationWrongPasswordTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequest("valaki@g.com", "valaki", "Va");

        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync("/Auth/Register", request);
        
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode );
    }
}