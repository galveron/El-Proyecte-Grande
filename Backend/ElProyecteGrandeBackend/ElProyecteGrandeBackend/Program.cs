using System.Text.Json.Serialization;
using AskMate.Service;
using ElProyecteGrandeBackend.Services.Repositories;

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, "connectionString.env");
Console.WriteLine(dotenv);
DotEnv.Load(dotenv);

var config =
    new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        policy  =>
        {
            policy.WithOrigins("*");
        });
});

var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();