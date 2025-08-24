using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        return new OkObjectResult("");
    }
}
