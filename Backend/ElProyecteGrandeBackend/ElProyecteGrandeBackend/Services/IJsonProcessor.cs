using ElProyecteGrandeBackend.Model;

namespace WeatherApi.Services;

public interface IJsonProcessor
{
    Order ProcessOrder(string data);
}