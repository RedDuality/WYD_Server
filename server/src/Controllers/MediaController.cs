using Microsoft.AspNetCore.Mvc;
using Core.DTO.MediaAPI;
using Core.Services.Events;
using Microsoft.AspNetCore.Authorization;
using server.Middleware;
namespace server.Controllers;

[ApiController]
[Route("Media")]
public class MediaController(ContextManager contextManager, EventService eventService) : ControllerBase
{

    [Authorize]
    [HttpPost("Event/getUploadUrls")]
    public async Task<IActionResult> GetUploadUrls([FromBody] MediaUploadRequestDto retrieveTokensDto)
    {
        var profile = await contextManager.GetCurrentProfile();
        var urls = await eventService.GetMediaUploadUrlsAsync(profile, retrieveTokensDto);
        return new OkObjectResult(urls);
    }

    [Authorize]
    [HttpPost("Events/getReadUrls")]
    public async Task<IActionResult> GetReadUrls([FromBody] MediaReadRequestDto mediaReadRequestDto)
    {
        var profile = await contextManager.GetCurrentProfile();
        var urls = await eventService.GetMediaReadUrlsAsync(profile, mediaReadRequestDto);
        return new OkObjectResult(urls);
    }
}
