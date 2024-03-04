using Microsoft.AspNetCore.Identity;

namespace ElProyecteGrandeBackend.Data;

public class AuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfigurationRoot _config;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
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
        await _roleManager.CreateAsync(new IdentityRole(_config["AdminRole"]));
    }

    async Task CreateUserRole()
    {
        await _roleManager.CreateAsync(new IdentityRole(_config["UserRole"]));
    }
    
    async Task CreateCompanyRole()
    {
        var companyRole = Environment.GetEnvironmentVariable("COMPANYROLE");
        await _roleManager.CreateAsync(new IdentityRole(companyRole));
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    async Task CreateAdminIfNotExists()
    {
        var adminInDb = await _userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
            var adminCreated = await _userManager.CreateAsync(admin, _config["AdminPassword"]);

            if (adminCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, _config["AdminRole"]);
            }
        }
    }
}