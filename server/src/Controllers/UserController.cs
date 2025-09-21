using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.DTO.Model;
using Core.DTO.UserAPI;
using server.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers;


[ApiController]
[Route("User")]
public class UserController(ContextManager contextManager, UserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet("Retrieve")]
    public async Task<IActionResult> GetUserAsync()
    {
        var currentUser = await contextManager.GetCurrentUser();
        var profiles = await userService.RetrieveProfilesAsync(currentUser);
        var userDto = new UserDto(currentUser, profiles);
        return new OkObjectResult(userDto);
    }

    [Authorize]
    [HttpPost("StoreFCMToken")]
    public async Task<IActionResult> StoreFcmToken(StoreFcmTokenRequestDto storeTokenDto)
    {
        var currentUser = await contextManager.GetCurrentUser();

        await userService.AddDevice(currentUser, storeTokenDto);
        return new OkObjectResult("");
    }
}
