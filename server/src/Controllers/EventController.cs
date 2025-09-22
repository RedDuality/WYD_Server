using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.DTO.EventAPI;

using Microsoft.AspNetCore.Authorization;
using server.Middleware;
using Amazon;

namespace server.Controllers;

[ApiController]
[Route("Event")]
public class EventController(ContextManager contextManager, EventService eventService) : ControllerBase
{
    private readonly EventService eventService = eventService;

    #region modify

    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateEventRequestDto newEvent)
    {
        var profileHash = contextManager.GetCurrentProfileId();
        var ev = await eventService.CreateEventAsync(newEvent, profileHash);
        return new OkObjectResult(ev);
    }

    [Authorize]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateEventRequestDto updateDto)
    {
        var ev = await eventService.UpdateEventAsync(updateDto);
        return new OkObjectResult(ev);
    }

    [Authorize]
    [HttpGet("Confirm/{eventId}")]
    public async Task<IActionResult> Confirm(string eventId)
    {
        var profileHash = contextManager.GetCurrentProfileId();
        await eventService.Confirm(eventId, profileHash);
        return new OkObjectResult("");
    }

    [Authorize]
    [HttpGet("Decline/{eventId}")]
    public async Task<IActionResult> Decline(string eventId)
    {
        var profileHash = contextManager.GetCurrentProfileId();
        await eventService.Decline(eventId, profileHash);
        return new OkObjectResult("");
    }
    #endregion

    #region retrieve

    [Authorize]
    [HttpPost("ListByProfile")]
    public async Task<IActionResult> ListByProfile([FromBody] RetrieveMultipleEventsRequestDto retrieveDto)
    {
        var events = await eventService.RetrieveEventsByProfileIds(retrieveDto);

        return new OkObjectResult(events);
    }


    [HttpGet("retrieveEssentials/{eventId}")]
    public async Task<IActionResult> GetEssentialsAsync(string eventId)
    {
        var profileHash = contextManager.GetCurrentProfileId();
        var ev = await eventService.RetrieveEventById(eventId, profileHash);
        return new OkObjectResult(ev);
    }

    [HttpGet("retrieveDetails/{eventId}")]
    public async Task<IActionResult> GetDetailsAsync(string eventId)
    {
        var ev = await eventService.RetrieveEventWithDetailsById(eventId);
        return new OkObjectResult(ev);
    }

    #endregion

}
