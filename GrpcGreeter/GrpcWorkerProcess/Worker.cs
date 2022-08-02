using GrpcWorkerProcess;
using GrpcGreeterWorker;
using Microsoft.Extensions.Hosting;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcWorkerProcess
{
    public class Worker : BackgroundService 
    {
        private readonly ILogger<Worker> logger;
        private readonly Greeter.GreeterClient client;
        private ManualResetEvent workerStarted=new ManualResetEvent(false);
        private readonly IHostApplicationLifetime applicationLifetime;

        public Worker(ILogger<Worker> logger,IHostApplicationLifetime applicationLifetime,  Greeter.GreeterClient client)
        {
            this.logger = logger;
            this.applicationLifetime = applicationLifetime;
            this.client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            var receivedData = await client.SendCollectionDataAsync(new Empty(), new CallOptions());
            this.logger.LogInformation("Message received from server:");
            foreach (var message in receivedData.Messages)
            {
                this.logger.LogInformation(message);
            }
            this.logger.LogInformation("Greeting: " + reply.Message);

            var getPeople = await client.GetAllPeopleAsync(new Empty(), new CallOptions());
            switch (getPeople.ResultCase)
            {
                case GetAllPeopleResponse.ResultOneofCase.Error:
                    this.logger.LogInformation($"Error retrieving result -{getPeople.Error.ErrorMessage}");
                    break;
                case GetAllPeopleResponse.ResultOneofCase.People:
                    await HandleGetAllPeople(this.logger, getPeople.People);
                    break;

            }
            workerStarted.WaitOne();
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            applicationLifetime.ApplicationStopped.Register(() => applicationLifetime.StopApplication());            
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            workerStarted.Set();
            await base.StopAsync(cancellationToken);
        }     

         private static async Task HandleGetAllPeople(ILogger<Worker> logger, People people)
        {
            foreach (var person in people.GetAll)
            {
                logger.LogInformation($"Name:{person.Name}, Age: {person.Age}");
            }
            await Task.CompletedTask;
        }   
    }
}
