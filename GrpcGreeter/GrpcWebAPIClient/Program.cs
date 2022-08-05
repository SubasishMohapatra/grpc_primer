using GrpcWebAPIClient;
using Microsoft.AspNetCore.Builder;
using GrpcWeatherServer;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<WeatherAppService>();
        builder.Services.AddGrpcClient<WeatherService.WeatherServiceClient>("WeatherServiceClient", o =>
               {
                //    o.Address = new Uri("http://localhost:5249");
                   o.Address = new Uri("https://localhost:7238");
               });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}