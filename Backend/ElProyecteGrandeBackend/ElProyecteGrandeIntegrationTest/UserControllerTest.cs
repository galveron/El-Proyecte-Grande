using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using ElProyecteGrandeBackend.Contracts;
using ElProyecteGrandeBackend.Model;
using ElProyecteGrandeBackend.Services;
using Microsoft.AspNetCore.TestHost;

namespace ElProyecteGrandeIntegrationTest;

[Collection("Integration")]
public class UserControllerTest
{
    [Fact]
    public async Task GetUserByUserNameValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //Login admin
        var loginReq = new AuthRequest("admin@admin.hu", "Adminadmin123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        var response = await client.GetAsync("/User/GetUser");
        
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        Assert.Equal("admin", authResponse.UserName);
    }
    
    [Fact]
    public async Task DeleteUserValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //Register customer
        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        //Login customer
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);

        //Get id to delete
        var requestUser = await client.GetAsync("/User/GetUser");
        var user = await requestUser.Content.ReadFromJsonAsync<User>();

        //Logout customer
        await client.PostAsJsonAsync("/Auth/Logout", new {});
        
        //Login admin
        var loginReq = new AuthRequest("admin@admin.hu", "Adminadmin123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        var response = await client.DeleteAsync($"/User/DeleteUserForAdmin?id={user.Id}");
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteUserInvalidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //Login admin
        var loginReq = new AuthRequest("admin@admin.hu", "Adminadmin123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);

        var response = await client.DeleteAsync($"/User/DeleteUserForAdmin?id=0");
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUserCustomerValid()
    {
        var application = new MarketPlaceWebApplicationFactory();

        var client = application.CreateClient();

        //Register customer
        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);

        //Login customer
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        var res = await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);

        var response =
            await client.PatchAsJsonAsync("/User/DeleteUser",
                new { });

        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCustomerValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        //Register customer
        var request = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", request);
        
        //Login customer
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);
        
        var response = await client.PatchAsJsonAsync("/User/UpdateCustomer?userName=bodybody&email=body%40body.hu&phoneNumber=123", new { });
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task UpdateCompanyValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        //Register company
        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        //Login company
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);

        var response = await client.PatchAsJsonAsync("/User/UpdateCompany?userName=bodybody&email=body%40body.hu&phoneNumber=123&companyName=bodyshop&identifier=123", new { });
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
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

        //Register company
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

        //Regsiter company
        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        //Login admin
        var loginReq = new AuthRequest("admin@admin.hu", "Adminadmin123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);

        var response = await client.PatchAsJsonAsync("/User/VerifyCompany?userName=body&verified=true", new { });
        
        response.EnsureSuccessStatusCode();
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
        
        //Register company
        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        //Login company
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);
        
        //Add product as company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logout company
        await client.PostAsJsonAsync("/Auth/Logout", new {});
        
        //Register user
        var requestCustomer = new RegistrationRequest("bodybody@body.com", "bodybody", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestCustomer);
        
        //Login user
        var loginBodyReqCustomer = new AuthRequest("bodybody@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReqCustomer);
        
        //Add product to favorites
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?productId=1", new { });
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddFavouriteRepeat()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //Register company
        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        //Login company
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);
        
        //Add product as company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logout company
        await client.PostAsJsonAsync("/Auth/Logout", new {});
        
        //Register user
        var requestCustomer = new RegistrationRequest("bodybody@body.com", "bodybody", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestCustomer);
        
        //Login user
        var loginBodyReqCustomer = new AuthRequest("bodybody@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReqCustomer);
        
        //Add product to favorites
        await client.PatchAsJsonAsync($"/User/AddFavourite?productId=1", new { });
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?productId=1", new { });

        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task RemoveFavouriteValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //Register company
        var request = new RegistrationRequestCompany("body@body.com", "body", "Bodybody123", "bodyshop", "123");
        await client.PostAsJsonAsync("/Auth/RegisterCompany", request);
        
        //Login company
        var loginBodyReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReq);
        
        //Add product as company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logout company
        await client.PostAsJsonAsync("/Auth/Logout", new {});
        
        //Register user
        var requestCustomer = new RegistrationRequest("bodybody@body.com", "bodybody", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestCustomer);
        
        //Login user
        var loginBodyReqCustomer = new AuthRequest("bodybody@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginBodyReqCustomer);
        
        //Add product to favorites
        var response = await client.PatchAsJsonAsync($"/User/AddFavourite?productId=1", new { });
        await client.PatchAsJsonAsync($"/User/RemoveFavourite?productId=1", new { });
        
        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsAddValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        //Register customer
        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        //Register company
        var requestRegComp = new RegistrationRequestCompany("valaki@g.com", "valaki", "Valaki123456", "ceg", "123");
        var regRequest = await client.PostAsJsonAsync("/Auth/RegisterCompany", requestRegComp);
        regRequest.EnsureSuccessStatusCode();
        
        //Login company
        var loginReqComp = new AuthRequest("valaki@g.com", "Valaki123456");
        var login = await client.PostAsJsonAsync("/Auth/Login", loginReqComp);
        login.EnsureSuccessStatusCode();
        
        //Add product as company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logout company
        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        //Login customer
        var loginReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        //Add item to cart
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?productId=1&quantity=1", new { });
        
        //Logout
        await client.PostAsJsonAsync("/Auth/Logout", "");

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsQuantityZero()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();
        
        //RegisterCustomer
        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        //Register company
        var requestRegComp = new RegistrationRequestCompany("valaki@g.com", "valaki", "Valaki123456", "ceg", "123");
        var regRequest = await client.PostAsJsonAsync("/Auth/RegisterCompany", requestRegComp);
        regRequest.EnsureSuccessStatusCode();
        
        //Login company
        var loginReqComp = new AuthRequest("valaki@g.com", "Valaki123456");
        var login = await client.PostAsJsonAsync("/Auth/Login", loginReqComp);
        login.EnsureSuccessStatusCode();
        
        //Add product by company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logout company
        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        //Login customer
        var loginReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        //Add item to cart
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?productId=1&quantity=0", new { });
        
        //Logut customer
        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task AddOrRemoveCartItemsRemoveValidUserName()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        //Register customer
        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);
        
        //Register company
        var requestRegComp = new RegistrationRequestCompany("valaki@g.com", "valaki", "Valaki123456", "ceg", "123");
        var regRequest = await client.PostAsJsonAsync("/Auth/RegisterCompany", requestRegComp);
        regRequest.EnsureSuccessStatusCode();
        
        //Login company
        var loginReqComp = new AuthRequest("valaki@g.com", "Valaki123456");
        var login = await client.PostAsJsonAsync("/Auth/Login", loginReqComp);
        login.EnsureSuccessStatusCode();
        
        //Add product by company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Logut company
        await client.PostAsJsonAsync("/Auth/Logout", "");
        
        //Login customer
        var loginReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        //Add and remove item from cart
        var response = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?productId=1&quantity=1", new { });
        var response2 = await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?productId=1&quantity=-1", new { });
        
        //Logout customer
        await client.PostAsJsonAsync("/Auth/Logout", "");

        response.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task EmptyCartWithLoggedInUser_EmptiesCart()
    {
        var application = new MarketPlaceWebApplicationFactory();
        
        var client = application.CreateClient();

        //Register customer
        var requestRegister = new RegistrationRequest("body@body.com", "body", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Register", requestRegister);

        //Login customer
        var loginReq = new AuthRequest("body@body.com", "Bodybody123");
        await client.PostAsJsonAsync("/Auth/Login", loginReq);
        
        var responseUser =  await client.GetAsync($"/User/GetUser");
        
        var user = await responseUser.Content.ReadFromJsonAsync<User>();
        var id = user.Id;
        
        //Add product by company
        await client.PostAsJsonAsync(
            $"/Product/AddProduct?userId={id}&name=product&price=2&details=nothing&quantity=3",
            new { });
        
        //Add product to cart
        await client.PatchAsJsonAsync($"/User/AddOrRemoveCartItems?userName=body&productId=1&quantity=1", new { });
        
        //Empty cart
        var response = await client.PatchAsJsonAsync($"/User/EmptyCart?userName=body", new { });

        //Logout
        await client.PostAsJsonAsync("Auth/Logout", "");
        
        response.EnsureSuccessStatusCode();
    }
}