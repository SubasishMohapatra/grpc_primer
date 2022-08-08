using Microsoft.AspNetCore.Mvc;
using GrpcWeatherServer;
using System.Text.Json;

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
        this.weatherAppService = weatherAppService;
    }

    [HttpGet("GetWeatherForecast")]
    public Task<(string, Google.Protobuf.Collections.RepeatedField<GrpcWeatherServer.Weather>)> GetWeatherForecast()
    {
        return this.weatherAppService.GetWeatherForecaseForNext5Days();
    }

    // [HttpGet("StreamWeatherForecast")]
    // public async Task<string> StreamWeatherForecast()
    // {
    //     var response = await this.weatherAppService.GetWeatherForecaseForXDays();
    //     return string.IsNullOrEmpty(response.errorMessage) ? response.errorMessage : response.weatherData;
    // }

    [HttpGet("StreamWeatherForecast")]
    [Produces("application/json")]
    public async Task<IActionResult> StreamWeatherForecast()
    {
        var response = await this.weatherAppService.GetWeatherForecaseForXDays();
        var result = string.IsNullOrEmpty(response.errorMessage)==false ? response.errorMessage : response.weatherData;
        return Ok(result);
    //     return new JsonResult(
    //    result,
    //    new JsonSerializerOptions
    //    {
    //        PropertyNamingPolicy = null
    //    });
    }
}
