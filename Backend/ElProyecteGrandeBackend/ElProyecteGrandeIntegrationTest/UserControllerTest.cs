using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services;
using Microsoft.AspNetCore.TestHost;

namespace ElProyecteGrandeIntegrationTest;

[Collection("Integration")]
public class UserControllerTest
{
    [Fact]
    public async Task GetUserByIdValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.GetAsync("/User/GetUser?userName=admin");
        
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        
        Assert.Equal("admin", authResponse.UserName);
    }
    
    [Fact]
    public async Task GetUserByIdInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.GetAsync("/User/GetUser?userName=nobody");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetUsers()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.GetAsync("/User/GetUsers");
        
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
        
        Assert.Equal(1, authResponse.Count());
    }
    
    [Fact]
    public async Task GetUsersNoUser()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        await client.DeleteAsync("/User/DeleteUser?userName=admin");
        var response = await client.GetAsync("/User/GetUsers");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUserValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);

        var response = await client.DeleteAsync("/User/DeleteUser?userName=body");
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteUserInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);

        var response = await client.DeleteAsync("/User/DeleteUser?userName=nobody");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCustomerValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCustomer?oldUserName=body&newUserName=bodybody&email=body%40body.hu&phoneNumber=123", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task UpdateCustomerInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCustomer?oldUserName=nobody&newUserName=bodybody&email=body%40body.hu&phoneNumber=123", new {  });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    
    [Fact]
    public async Task UpdateCustomerSameUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCustomer?oldUserName=body&newUserName=body&email=body%40body.hu&phoneNumber=123", new {  });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCompanyValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCompany?oldUserName=body&newUserName=bodybody&email=body%40body.hu&phoneNumber=123&companyName=bodyshop&identifier=123", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task UpdateCompanyInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCompany?oldUserName=nobody&newUserName=bodybody&email=body%40body.hu&phoneNumber=123&companyName=bodyshop&identifier=123", new {  });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCompanyBadRequest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.PatchAsJsonAsync("", new {  });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task VerifyCompanyValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        var response = await client.PatchAsJsonAsync("/User/VerifyCompany?UserName=body&verified=true", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task VerifyCompanyInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        var response = await client.PatchAsJsonAsync("/User/VerifyCompany?UserName=nobody&verified=true", new { });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task VerifyCompanyBadRequest()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var response = await client.PatchAsJsonAsync("", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task AddFavouriteValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        var responseRegister = await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?UserName=body&productId=1", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddFavouriteInvalidProduct()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?UserName=body&productId=0", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task RemoveFavouriteValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        var responseRegister = await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?UserName=body&productId=1", new { });
        
        var response2 = await client.PatchAsJsonAsync($"/User/RemoveFavourite?UserName=body&productId=1", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task RemoveFavouriteInvalidProduct()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        var response = await client.PatchAsJsonAsync($"/User/RemoveFavourite?UserName=body&productId=0", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsAddValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        
        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task AddOrRemoveCartItemsRemoveValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        var response2 = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        
        response2.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsAddInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId=1&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        var response2 = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsRemoveInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId=1&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task EmptyCartValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        var responseUser =  await client.GetAsync(
            $"/User/GetUser?userName=body");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        var response = await client.PatchAsJsonAsync($"/User/EmptyCart?userName=body", new { });
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task EmptyCartInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId=1&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        var response = await client.PatchAsJsonAsync($"/User/EmptyCart?userName=body", new { });
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}