using Microsoft.AspNetCore.Mvc;
using Core.DTO.MediaAPI;
using Core.Services.Events;
using Microsoft.AspNetCore.Authorization;
using Core.Services.Profiles;
using Core.Services.Util;
namespace server.Controllers;

[ApiController]
[Route("Media")]
public class MediaController(IContextManager contextManager, ProfileService profileService, EventService eventService) : ControllerBase
{

    [Authorize(policy: "CanEditEvents")]
    [HttpPost("Event/getUploadUrls")]
    public async Task<IActionResult> GetUploadUrls([FromBody] MediaUploadRequestDto retrieveTokensDto)
    {
        // u admin
        // u partecipant
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var urls = await eventService.GetMediaUploadUrlsAsync(profile, retrieveTokensDto);
        return new OkObjectResult(urls);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpPost("Events/getReadUrls")]
    public async Task<IActionResult> GetReadUrls([FromBody] MediaReadRequestDto mediaReadRequestDto)
    {
        // u viewer
        // u partecipant
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var urls = await eventService.GetMediaReadUrlsAsync(profile, mediaReadRequestDto);
        return new OkObjectResult(urls);
    }
}
