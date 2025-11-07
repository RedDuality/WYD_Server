using Microsoft.AspNetCore.Mvc;
using Core.Services.Users;
using Core.DTO.UserAPI;
using server.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers;

[ApiController]
[Route("User")]
public class UserController(
    ContextManager contextManager,
    UserService userService,
    DeviceService deviceService) : ControllerBase
{
    [Authorize]
    [HttpGet("Retrieve")]
    public async Task<IActionResult> GetUserAsync()
    {
        var currentUser = await contextManager.GetCurrentUser();
        var userDto = await userService.RetrieveProfilesAsync(currentUser);
        return new OkObjectResult(userDto);
    }

    [Authorize]
    [HttpPost("StoreFCMToken")]
    public async Task<IActionResult> StoreFcmToken(StoreFcmTokenRequestDto storeTokenDto)
    {
        // viewer
        var currentUser = await contextManager.GetCurrentUser();

        await deviceService.AddDevice(currentUser.Id, storeTokenDto);
        return new OkObjectResult("");
    }
}
