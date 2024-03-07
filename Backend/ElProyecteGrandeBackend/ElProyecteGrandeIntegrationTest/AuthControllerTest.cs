using System.Net;
using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Model;
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

    [Fact]
    public async Task UserLoginSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var registrationRequest = new RegistrationRequest(testEmail, "valakik", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);

        var client = application.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/Auth/Register", registrationRequest);
        registerResponse.EnsureSuccessStatusCode();
        
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        
        var getUserRes = await client.GetAsync($"/User/GetUser");
        getUserRes.EnsureSuccessStatusCode();
        var user = await getUserRes.Content.ReadFromJsonAsync<User>();
        
        Assert.Equal(testEmail, user.Email);
    }
    
    [Fact]
    public async Task UserLoginWrongCredentialsTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        const string testEmail = "valaki@g.com";
        const string testPassword = "Valaki123456";
        var registrationRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest("something", "nothing");

        var client = application.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/Auth/Register", registrationRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);

        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
    }
    
    [Fact]
    public async Task CompanyRegistrationSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequestCompany("valaki23@g.com", "valaki23", "Valaki123", "valakiBT", "AB12345");

        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        
        Assert.Equal("valaki23@g.com", authResponse.Email);
    }

    [Fact]
    public async Task CompanyRegistrationWrongCredentialsTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequestCompany("valaki@g.com", "valaki", "Va", "", "");

        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode );
    }
}