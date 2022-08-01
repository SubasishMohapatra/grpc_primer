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

            var getPeople = await client.GetAllPeopleAsync(new Empty(), new CallOptions());
            switch (getPeople.ResultCase)
            {
                case GetAllPeopleResponse.ResultOneofCase.Error:
                    Console.WriteLine($"Error retrieving result -{getPeople.Error.ErrorMessage}");
                    break;
                case GetAllPeopleResponse.ResultOneofCase.People:
                    await HandleGetAllPeople(getPeople.People);
                    break;

            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static async Task HandleGetAllPeople(People people)
        {
            foreach (var person in people.GetAll)
            {
                Console.WriteLine($"Name:{person.Name}, Age: {person.Age}");
            }
            await Task.CompletedTask;
        }
    }
}