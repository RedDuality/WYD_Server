using Microsoft.AspNetCore.Mvc;
using Core.DTO.EventAPI;

using Microsoft.AspNetCore.Authorization;
using Core.Services.Profiles;
using Core.Services.Util;
using Core.Services.Events.Recurrence;

namespace server.Controllers;

[ApiController]
[Route("Event/Recurrent")]
public class RecurrentEventController(
    IContextManager contextManager,
    ProfileService profileService,
    RecurrentEventService eventService) : ControllerBase
{

    [Authorize(policy: "CanCreateEvents")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateRecurrentEventRequestDto newEvent)
    {
        // User Admin
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var ev = await eventService.CreateRecurrentEventAsync(newEvent, profile);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpPost("retrieveDetails")]
    public async Task<IActionResult> GetDetailsAsync([FromBody] RetrieveRecurrenceInstanceDetailsRequestDto requestDto)
    {
        // u viewer
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var details = await eventService.RetrieveDetailsById(profile, requestDto);
        return new OkObjectResult(details);
    }

}
