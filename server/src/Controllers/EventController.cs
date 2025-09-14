using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.DTO.EventAPI;

using Microsoft.AspNetCore.Authorization;
using server.Middleware;

namespace server.Controllers;

[ApiController]
[Route("Event")]
public class EventController(ContextManager contextManager, EventService eventService) : ControllerBase
{
    private readonly EventService eventService = eventService;

    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateEventRequestDto newEvent)
    {
        var profileHash = contextManager.GetCurrentProfileId();
        var ev = await eventService.CreateEventAsync(newEvent, profileHash);
        return new OkObjectResult(ev);
    }

    [Authorize]
    [HttpPost("ListByProfile")]
    public async Task<IActionResult> ListByProfile([FromBody] RetrieveMultipleEventsRequestDto retrieveDto)
    {
        var events = await eventService.RetrieveEventsByProfileId(retrieveDto);

        return new OkObjectResult(events);
    }


    [HttpGet("retrieve/{hash}")]
    public async Task<IActionResult> GetAsync(string hash)
    {
        var ev = await eventService.RetrieveEventById(hash);
        return new OkObjectResult(ev);
    }

}
