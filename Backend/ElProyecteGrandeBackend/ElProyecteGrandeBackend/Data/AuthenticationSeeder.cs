using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Identity;

namespace ElProyecteGrandeBackend.Data;

public class AuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfigurationRoot _config;
    
    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _config = new ConfigurationBuilder()
            .AddUserSecrets<AuthenticationSeeder>()
            .Build();
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole();
        tAdmin.Wait();

        var tUser = CreateUserRole();
        tUser.Wait();
        
        var tCompany = CreateCompanyRole();
        tCompany.Wait();
    }

    async Task CreateAdminRole()
    {
        await _roleManager.CreateAsync(new IdentityRole(_config["AdminRole"]   != null ? _config["AdminRole"] : Environment.GetEnvironmentVariable("ADMINROLE")));
    }

    async Task CreateUserRole()
    {
        await _roleManager.CreateAsync(new IdentityRole(_config["CustomerRole"]  != null ? _config["CustomerRole"] : Environment.GetEnvironmentVariable("CUSTOMERROLE")));
    }
    
    async Task CreateCompanyRole()
    {
        await _roleManager.CreateAsync(new IdentityRole(_config["CompanyRole"]  != null ? _config["CompanyRole"] : Environment.GetEnvironmentVariable("COMPANYROLE")));
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    async Task CreateAdminIfNotExists()
    {
        var adminInDb = await _userManager.FindByEmailAsync("admin@admin.hu");
        if (adminInDb == null)
        {
            var admin = new User { UserName = "admin", Email = "admin@admin.hu" };
            var adminCreated = await _userManager.CreateAsync(admin, _config["AdminPassword"] != null ? _config["AdminPassword"] : Environment.GetEnvironmentVariable("ADMINPASSWORD"));

            if (adminCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, _config["AdminRole"] != null ? _config["AdminRole"] : Environment.GetEnvironmentVariable("ADMINROLE"));
            }
        }
    }
}