using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcGreeterServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override Task<SampleCollection> SendCollectionData(Empty request, ServerCallContext context)
    {
        var sampleData = new SampleCollection();
        for (var i = 0; i < 5; i++)
        {
            sampleData.Messages.Add($"Message {i}");
        }
        return Task.FromResult(sampleData);
    }

    public override Task<GetAllPeopleResponse> GetAllPeople(Empty request, ServerCallContext context)
    {
        var getAllPeopleResponseWithError = new GetAllPeopleResponse() {Error=new Error(){ErrorMessage="Bad data"} };
        return Task.FromResult(getAllPeopleResponseWithError);
        // var getAllPeopleResponse = new GetAllPeopleResponse() { People = new People() };
        // for (var i = 1; i <= 5; i++)
        // {
        //     getAllPeopleResponse.People.GetAll.Add(new Person() { Name = $"Person- {i}", Age = i * 10 });
        // }
        // return Task.FromResult(getAllPeopleResponse);
    }
}
