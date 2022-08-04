using Microsoft.AspNetCore.Mvc;

namespace GrpcWebAPIClient.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<PeopleController> _logger;

    public PeopleController(ILogger<PeopleController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "SayHello")]
    public string SayHello(string name)
    {
        return $"Hello {name}";
    }
}
