using GrpcWorkerProcess;
using GrpcGreeterWorker;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddGrpcClient<Greeter.GreeterClient>(o =>
        {
            o.Address = new Uri("https://localhost:7238");
        });
    })
    .Build();
        await host.RunAsync();
    }
}