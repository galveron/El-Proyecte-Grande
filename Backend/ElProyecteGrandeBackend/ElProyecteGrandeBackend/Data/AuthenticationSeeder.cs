using ElProyecteGrandeBackend.Model;
using Microsoft.AspNetCore.Identity;

namespace ElProyecteGrandeBackend.Data;

public class AuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
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
        await _roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    async Task CreateUserRole()
    {
        await _roleManager.CreateAsync(new IdentityRole("Customer"));
    }
    
    async Task CreateCompanyRole()
    {
        var companyRole = Environment.GetEnvironmentVariable("COMPANYROLE");
        await _roleManager.CreateAsync(new IdentityRole("Company"));
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
            var adminCreated = await _userManager.CreateAsync(admin, "admin123");

            if (adminCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}