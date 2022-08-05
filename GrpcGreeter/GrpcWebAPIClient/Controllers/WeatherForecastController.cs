using Microsoft.AspNetCore.Mvc;

namespace GrpcWebAPIClient.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<Weather> Get()
    // {
    //   return null;
    // }
}
