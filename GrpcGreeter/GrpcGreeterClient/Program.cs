using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcGreeterClient
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:7238");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            var receivedData = await client.SendCollectionDataAsync(new Empty(), new CallOptions());
            System.Console.WriteLine("Message received from server:");
            foreach (var message in receivedData.Messages)
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}