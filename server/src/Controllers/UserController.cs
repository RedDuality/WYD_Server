using Microsoft.AspNetCore.Mvc;
using Core.Services.Users;
using Core.DTO.UserAPI;
using server.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers;

[ApiController]
[Route("User")]
public class UserController(
    UserService userService,
    DeviceService deviceService) : ControllerBase
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


    [Authorize]
    [HttpPost("StoreFCMToken")]
    public async Task<IActionResult> StoreFcmToken(StoreFcmTokenRequestDto storeTokenDto)
    {
        await deviceService.AddDevice(storeTokenDto);
        return new OkObjectResult("");
    }

    [Authorize]
    [HttpPost("RemoveFCMToken")]
    public async Task<IActionResult> RemoveFcmToken(string fcmToken)
    {
        await deviceService.RemoveDevice(fcmToken);
        return new OkObjectResult("");
    }
}
