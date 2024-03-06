using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services;
using ElProyecteGrandeBackend.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ElProyecteGrandeIntegrationTest;
[Collection("Integration")]
public class OrderControllerTest
{
    [Fact]
    public async Task AddOrderSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        
        var response = await client.PostAsJsonAsync($"Order/AddOrder", "");

        response.EnsureSuccessStatusCode();
        
    }
    
    [Fact]
    public async Task AddOrderFailedTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        var response = await client.PostAsJsonAsync("Order/AddOrder", "22");
        
    }
    
    [Fact]
    public async Task GetOrderSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        
        var addResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");

        var getResponse = await client.GetAsync("Order/GetOrder?orderId=1");

        getResponse.EnsureSuccessStatusCode();

        var taskResult = getResponse.Content.ReadFromJsonAsync<Order>();
        
        Assert.NotNull(taskResult.Result);

    }
    
    [Fact]
    public async Task AddProductToOrderSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regCompanyRequest = new RegistrationRequestCompany("vk@g.com", "company", "companyPassword", "companyBT", "1");
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();

        var regCompanyResponse = await client.PostAsJsonAsync("Auth/RegisterCompany",regCompanyRequest);
        var loginCompanyResponse = await client.PostAsJsonAsync("Auth/Login",new AuthRequest("vk@g.com", "companyPassword"));
        var addProductResponse = await client.PostAsJsonAsync($"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3", "");
        addProductResponse.EnsureSuccessStatusCode();
        var logOutResponse = await client.PostAsJsonAsync("Auth/Logout", "");
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");

        var addProductToOrderResponse = await client.PostAsJsonAsync($"/Order/AddOrRemoveProductFromOrder?orderId=1&productId=1&quantity=2", "");

        addProductToOrderResponse.EnsureSuccessStatusCode();

    }
    
    [Fact]
    public async Task AddProductToOrderFailedTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");
        var addProductResponse = await client.PostAsJsonAsync($"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3", "");

        var addProductToOrderResponse = await client.PostAsJsonAsync($"/Order/AddOrRemoveProductFromOrder?orderId=10&productId=10&quantity=2", "");

        Assert.Equal(HttpStatusCode.InternalServerError,addProductToOrderResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetUserOrdersSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");

        var getUserOrderResponse = await client.GetAsync($"Order/GetUserOrders");

        getUserOrderResponse.EnsureSuccessStatusCode();

    }
    
    [Fact]
    public async Task GetUserOrdersFailedTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");

        var getUserOrderResponse = await client.GetAsync($"Order/GetUserOrders?userId=12");

        //Assert.Equal();

    }
    
    [Fact]
    public async Task EmptyOrderItemsSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");
        var addProductResponse = await client.PostAsJsonAsync($"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3", "");
        var addProductToOrderResponse = await client.PostAsJsonAsync($"/Order/AddOrRemoveProductFromOrder?orderId=1&productId=1&quantity=2", "");

        var emptyOrderItemsResponse = await client.PatchAsJsonAsync($"Order/EmptyOrderItems?orderId=1", "");

        emptyOrderItemsResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task EmptyOrderItemsFailedTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");
        var addProductResponse = await client.PostAsJsonAsync($"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3", "");
        var addProductToOrderResponse = await client.PostAsJsonAsync($"/Order/AddOrRemoveProductFromOrder?orderId=1&productId=1&quantity=2", "");
        
        var emptyOrderItemsResponse = await client.PatchAsJsonAsync($"Order/EmptyOrderItems?orderId=11", "");
    
        Assert.Equal(HttpStatusCode.InternalServerError,emptyOrderItemsResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteOrderSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder", "");

        var deleteResponse = await client.DeleteAsync($"Order/DeleteOrder?orderId=1");

        deleteResponse.EnsureSuccessStatusCode();
    }
    /*
    [Fact]
    public async Task UpdateOrderSuccessfullyTest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        const string testEmail = "valaki@gm.com";
        const string testPassword = "Valaki123456";
        var regRequest = new RegistrationRequest(testEmail, "valaki", testPassword);
        var loginRequest = new AuthRequest(testEmail, testPassword);
        
        var client = application.CreateClient();
        
        var regResponse = await client.PostAsJsonAsync("/Auth/Register", regRequest);
        var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        var addOrderResponse = await client.PostAsJsonAsync($"Order/AddOrder?userid={authResponse?.UserId}", authResponse?.UserId);
        var addProductResponse = await client.PostAsJsonAsync($"/Product/AddProduct?userId={authResponse?.UserId}&name=product&price=2&details=nothing&quantity=3", authResponse?.UserId);

        var testOrder = new Order{Date = DateTime.Now, PriceToPay = 100};

        var updateOrderResponse = await client.PatchAsJsonAsync($"Order/UpdateOrder?order={testOrder}",authResponse?.UserId);

        updateOrderResponse.EnsureSuccessStatusCode();

    }
    */
}