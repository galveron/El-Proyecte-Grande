using ElProyecteGrandeBackend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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
            services.RemoveAll<DbContextOptions<MarketPlaceContext>>();
            
            const string connectionString = "";
            services.AddSqlServer<MarketPlaceContext>(connectionString);

            var dbContext = CreateDbContext(services);
            
            //dbContext.Database.EnsureDeleted();
        });
    }

    private static MarketPlaceContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MarketPlaceContext>();
        return dbContext;
    }
}