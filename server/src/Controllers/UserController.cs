using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.Model.Dto;
using server.Middleware;

namespace server.Controllers;

[ApiController]
[Route("auth")]
public class UserController(ContextManager contextManager, UserService userService) : ControllerBase
{


    [HttpGet("Login")]
    public async Task<IActionResult> GetUserAsync()
    {
        var currentUser = await contextManager.GetCurrentUser();
        var profiles = await userService.RetrieveProfilesAsync(currentUser);
        var userDto = new UserDto(currentUser, profiles);
        return new OkObjectResult(userDto);
    }
}
