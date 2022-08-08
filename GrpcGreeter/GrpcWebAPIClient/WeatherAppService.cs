using Grpc.Net.ClientFactory;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcWeatherServer;
using Google.Protobuf;
using System.Text.Json;

namespace GrpcWebAPIClient
{
    public class WeatherAppService
    {
        private readonly WeatherService.WeatherServiceClient client;
        private readonly ILogger<WeatherAppService> logger;

        public WeatherAppService(ILogger<WeatherAppService> logger, GrpcClientFactory grpcClientFactory)
        {
            client = grpcClientFactory.CreateClient<WeatherService.WeatherServiceClient>("WeatherServiceClient");
            this.logger = logger;
        }

        public async Task<(string, Google.Protobuf.Collections.RepeatedField<GrpcWeatherServer.Weather>)> GetWeatherForecaseForNext5Days()
        {
            var errorMessage = "";
            var response = await client.GetWeatherReportForNext5DaysAsync(new Empty(), new CallOptions());
            Google.Protobuf.Collections.RepeatedField<GrpcWeatherServer.Weather> result = null;
            switch (response.ResultCase)
            {
                case GetWeatherForecastResponse.ResultOneofCase.Error:
                    errorMessage = $"Error retrieving result -{response.Error.ErrorMessage}";
                    break;
                case GetWeatherForecastResponse.ResultOneofCase.WeatherForeCast:
                    result = response.WeatherForeCast.Data;
                    break;
            }
            return (errorMessage, result);
        }

        // public async Task<(string, Google.Protobuf.Collections.RepeatedField<GrpcWeatherServer.Weather>)> GetWeatherForecaseForXDays()
        // {
        //     var errorMessage = "";
        //     var result = new Google.Protobuf.Collections.RepeatedField<Weather>();
        //     try
        //     {
        //         using var streamingCall = client.StreamWeatherReport(new Empty(), new CallOptions());
        //         await foreach (var weather in streamingCall.ResponseStream.ReadAllAsync())
        //         {
        //             result.Add(weather);
        //         }
        //     }
        //     catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        //     {
        //         errorMessage = "Stream cancelled.";
        //     }
        //     return (errorMessage, result);
        // }

         public async Task<(string errorMessage, string weatherData)> GetWeatherForecaseForXDays()
        {
            var errorMessage = "";
            var result = new Google.Protobuf.Collections.RepeatedField<Weather>();
            try
            {
                using var streamingCall = client.StreamWeatherReport(new Empty(), new CallOptions());
                await foreach (var weather in streamingCall.ResponseStream.ReadAllAsync())
                {
                    result.Add(weather);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                errorMessage = "Stream cancelled.";
            }
            var jsonResponse=JsonSerializer.Serialize(result);
            return (errorMessage, jsonResponse);
        }
    }
}