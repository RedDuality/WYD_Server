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
    [HttpGet("Register")]
    public async Task<IActionResult> Register()
    {
        var accountId = contextManager.GetAccountId();
        var email = contextManager.GetEmail();

        var userDto = await userService.CreateUserAsync(accountId, email);

        return new OkObjectResult(userDto);
    }

    [Authorize]
    [HttpGet("Login")]
    public async Task<IActionResult> Login()
    {
        var accountId = contextManager.GetAccountId();
        var userId = contextManager.GetUserId();

        var userDto = await userService.RetrieveWithProfiles(userId, accountId);

        return new OkObjectResult(userDto);
    }


    [Authorize]
    [HttpPost("StoreFCMToken")]
    public async Task<IActionResult> StoreFcmToken(StoreFcmTokenRequestDto storeTokenDto)
    {
        // viewer
        var accountId = contextManager.GetAccountId();
        var userId = contextManager.GetUserId();

        var currentUser = await userService.Retrieve(userId, accountId);

        await deviceService.AddDevice(currentUser.Id, storeTokenDto);
        return new OkObjectResult("");
    }
}
