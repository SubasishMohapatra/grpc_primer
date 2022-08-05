using Microsoft.AspNetCore.Mvc;
using GrpcWeatherServer;

namespace GrpcWebAPIClient.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly WeatherAppService weatherAppService;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherAppService weatherAppService)
    {
        logger = logger;
        this.weatherAppService=weatherAppService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public  Task<(string, Google.Protobuf.Collections.RepeatedField<GrpcWeatherServer.Weather>)> Get()
    {
      return this.weatherAppService.GetWeatherForecaseForNext5Days();
    }
}
