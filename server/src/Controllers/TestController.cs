using Microsoft.AspNetCore.Mvc;
namespace server.Controllers;


[ApiController]
[Route("Test")]
public class TestController() : ControllerBase
{
    [HttpGet("Ping")]
    public  IActionResult Ping()
    {
        return new OkObjectResult("The server is online and waiting");
    }
}
