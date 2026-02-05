using Microsoft.AspNetCore.Mvc;
using Core.Services.Events;
using Core.DTO.EventAPI;
using Core.DTO.CommunityAPI;

using Microsoft.AspNetCore.Authorization;
using Core.Services.Profiles;
using Core.Services.Util;

namespace server.Controllers;

[ApiController]
[Route("Event")]
public class EventController(IContextManager contextManager, ProfileService profileService, EventService eventService) : ControllerBase
{
    private readonly EventService eventService = eventService;

    #region modify

    [Authorize(policy: "CanCreateEvents")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateEventRequestDto newEvent)
    {
        // User Admin
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var ev = await eventService.CreateEventAsync(newEvent, profile);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanEditEvents")]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateEventRequestDto updateDto)
    {
        // u admin
        // e admin
        var ev = await eventService.UpdateEventAsync(updateDto);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanShareEvents")]
    [HttpPost("Share/{eventId}")]
    public async Task<IActionResult> Share(string eventId, [FromBody] List<ShareEventRequestDto> dtos)
    {
        // u admin
        // e partecipant
        var profileId = contextManager.GetCurrentProfileId();
        var profile = await profileService.RetrieveProfileById(profileId);
        var ev = await eventService.ShareEventAsync(profile, eventId, dtos);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanEditEvents")]
    [HttpGet("Confirm/{eventId}")]
    public async Task<IActionResult> Confirm(string eventId)
    {
        // u admin
        // e partecipant
        var profileHash = contextManager.GetCurrentProfileId();
        await eventService.Confirm(eventId, profileHash);
        return new OkObjectResult("");
    }

    [Authorize(policy: "CanEditEvents")]
    [HttpGet("Decline/{eventId}")]
    public async Task<IActionResult> Decline(string eventId)
    {
        // u admin
        // e partecipant
        var profileHash = contextManager.GetCurrentProfileId();
        await eventService.Decline(eventId, profileHash);
        return new OkObjectResult("");
    }

    #endregion

    #region retrieve

    [Authorize(policy: "CanReadEvents")]
    [HttpPost("ListByProfile")]
    public async Task<IActionResult> ListByProfile([FromBody] RetrieveMultipleEventsRequestDto retrieveDto)
    {
        // u viewer
        var events = await eventService.RetrieveEventsByProfileIds(retrieveDto);

        return new OkObjectResult(events);
    }


    [Authorize(policy: "CanReadEvents")]
    [HttpPost("UpdateByProfile")]
    public async Task<IActionResult> UpdateByProfile([FromBody] RetrieveMultipleEventsRequestDto retrieveDto)
    {
        // u viewer
        var events = await eventService.RetrieveUpdatesByProfileIds(retrieveDto);

        return new OkObjectResult(events);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpGet("retrieveEssentials/{eventId}")]
    public async Task<IActionResult> GetEssentialsAsync(string eventId)
    {
        // u viewer
        var profileHash = contextManager.GetCurrentProfileId();
        var ev = await eventService.RetrieveEventById(eventId, profileHash);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpGet("retrieveDetails/{eventId}")]
    public async Task<IActionResult> GetDetailsAsync(string eventId)
    {
        // u viewer
        var ev = await eventService.RetrieveEventWithDetailsById(eventId);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpGet("retrieveFromShared/{eventId}")]
    public async Task<IActionResult> RetrieveFromShared(string eventId)
    {
        // u viewer
        var profileHash = contextManager.GetCurrentProfileId();
        var ev = await eventService.CreateAndRetrieveSharedEvent(eventId, profileHash);
        return new OkObjectResult(ev);
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpGet("getProfileEvents/{eventId}")]
    public async Task<IActionResult> GetProfileEventsAsync(string eventId)
    {
        // u partecipant
        var ev = await eventService.GetProfileEventsAsync(eventId);
        return new OkObjectResult(ev);
    }

    #endregion

}
