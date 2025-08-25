using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.Model;
using Core.Model.Dto;
using server.Middleware;
using System.Threading.Tasks;

namespace server.Controllers;

[ApiController]
[Route("auth")]
public class UserController(UserService userService) : ControllerBase
{

//eliminare
    [HttpGet("VerifyToken")]
    public async Task<IActionResult> Get()
    {
        return await GetUserAsync();
    }

    [HttpGet("Login")]
    public async Task<IActionResult> GetUserAsync()
    {
        var currentUser = HttpContext.GetUser();
        var profiles = await userService.RetrieveProfilesAsync(currentUser);
        var userDto = new UserDto(currentUser, profiles);
        return new OkObjectResult(userDto);
    }
}
