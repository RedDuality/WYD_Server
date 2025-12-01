using Microsoft.AspNetCore.Mvc;
using Core.Services.Users;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers;

[ApiController]
[Route("User")]
public class UserController(
    UserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet("Register")]
    public async Task<IActionResult> Register()
    {
        var userDto = await userService.Register();

        return new OkObjectResult(userDto);
    }

    [Authorize]
    [HttpGet("Login")]
    public async Task<IActionResult> Login()
    {
        var userDto = await userService.Login();

        return new OkObjectResult(userDto);
    }
}
