using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services;

namespace ElProyecteGrandeIntegrationTest;

[Collection("Integration")]
public class ProductControllerTest
{
    [Fact]
    public async Task GetAllProductsReturnsAllProducts()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.GetAsync("/Product/GetAllProducts");
        
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        
        Assert.NotNull(authResponse);
    }

    [Fact]
    public async Task GetProductReturnsExpectedProduct()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequestCompany("valaki@g.com", "valaki", "Valaki123456", "ceg", "123");
        
        var client = application.CreateClient();

        var regRequest = await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        regRequest.EnsureSuccessStatusCode();
        var authResponse = await regRequest.Content.ReadFromJsonAsync<RegistrationResponseCompany>();

        var loginReq = new AuthRequest("valaki@g.com", "Valaki123456");
        var login = await client.PostAsJsonAsync("/Auth/Login", loginReq);
        login.EnsureSuccessStatusCode();

        var getUserRes = await client.GetAsync($"/User/GetUser");
        getUserRes.EnsureSuccessStatusCode();
        var user = await getUserRes.Content.ReadFromJsonAsync<User>();
        
        var product = new Product { Name = "term", Seller = user, Price = 123, Details = "rrr", Quantity = 2 };
        var addProductRes = await client.PostAsJsonAsync(
            $"/Product/AddProduct?name={product.Name}&price={product.Price}&details={product.Details}&quantity={product.Quantity}",
            new { });
        addProductRes.EnsureSuccessStatusCode();

        var productRes = await client.GetAsync("/Product/GetProduct?id=1");
        productRes.EnsureSuccessStatusCode();
        var response = await productRes.Content.ReadFromJsonAsync<Product>();

        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        Assert.Equal("term", response.Name);
    }

    [Fact]
    public async Task AddProduct_SuccessfullyAddsProduct()
    {
        var application = new MarketPlaceWebApplicationFactory();
        var request = new RegistrationRequestCompany("valaki@g.com", "valaki", "Valaki123456", "ceg", "123");
        
        var client = application.CreateClient();

        var regRequest = await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        regRequest.EnsureSuccessStatusCode();
        var authResponse = await regRequest.Content.ReadFromJsonAsync<RegistrationResponseCompany>();

        var loginReq = new AuthRequest("valaki@g.com", "Valaki123456");
        var login = await client.PostAsJsonAsync("/Auth/Login", loginReq);
        login.EnsureSuccessStatusCode();

        var getUserRes = await client.GetAsync($"/User/GetUser");
        getUserRes.EnsureSuccessStatusCode();
        var user = await getUserRes.Content.ReadFromJsonAsync<User>();
        
        var product = new Product { Name = "term", Seller = user, Price = 123, Details = "rrr", Quantity = 2 };
        var addProductRes = await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={user.Id}&name={product.Name}&price={product.Price}&details={product.Details}&quantity={product.Quantity}",
            new { });
        addProductRes.EnsureSuccessStatusCode();
        var addedProduct = await addProductRes.Content.ReadFromJsonAsync<Product>();
        
        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        Assert.Equal("term", addedProduct.Name);
    }
}