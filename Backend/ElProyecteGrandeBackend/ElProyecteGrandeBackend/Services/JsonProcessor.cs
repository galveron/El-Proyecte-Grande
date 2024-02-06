using System.Text.Json;
using ElProyecteGrandeBackend.Model;
using WeatherApi.Services;

namespace ElProyecteGrandeBackend.Services;

public class JsonProcessor : IJsonProcessor
{
    public Order ProcessOrder(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        var products = new List<Product>();
        var enumerator = json.RootElement.GetProperty("products").EnumerateArray();
    
        var order = new Order(
            json.RootElement.GetProperty("id").GetInt32(),
            json.RootElement.GetProperty("userId").GetInt32(),
            json.RootElement.GetProperty("date").GetDateTime(),
            products
            );
        
        return order;
    }
}