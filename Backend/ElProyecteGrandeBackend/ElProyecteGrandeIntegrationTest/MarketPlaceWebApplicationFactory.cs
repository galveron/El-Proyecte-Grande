using ElProyecteGrandeBackend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ElProyecteGrandeIntegrationTest;

public class MarketPlaceWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<MarketPlaceContext>));

            services.Remove(dbContextDescriptor);
            
            var config = new ConfigurationBuilder()
                .AddUserSecrets<MarketPlaceWebApplicationFactory>()
                .Build();
            
            services.AddDbContext<MarketPlaceContext>((container, options) =>
            {
                options.UseSqlServer(config["TestConnectionString"]);
            });

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MarketPlaceContext>();
            var authSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            dbContext.Database.EnsureCreated();
            authSeeder.AddRoles();
            authSeeder.AddAdmin();
        });
    }
}