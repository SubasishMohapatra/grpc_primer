using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWeatherServer;
using Grpc.Domain;

namespace GrpcWeatherServer.Services;

public class WeatherForecastService : WeatherService.WeatherServiceBase
{
     private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastService> _logger;
    public WeatherForecastService(ILogger<WeatherForecastService> logger)
    {
        _logger = logger;
    }
    
    public override Task<GetWeatherForecastResponse> GetWeatherReportForNext5Days(Empty request, ServerCallContext context)
    {
        var data= Enumerable.Range(1, 5).Select(index => new Weather
        {
            Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
        var response=new GetWeatherForecastResponse(){WeatherForeCast=new WeatherForeCast()};
        response.WeatherForeCast.Data.AddRange(data);
        return Task.FromResult<GetWeatherForecastResponse>(response);
    }
}
