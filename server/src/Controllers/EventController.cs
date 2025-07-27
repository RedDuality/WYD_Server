using Microsoft.AspNetCore.Mvc;
using Core.Services.Model;
using Core.Model;
using Core.Model.Dto;

namespace server.Controllers;

[ApiController]
[Route("api/event")]
public class EventController(EventService eventService) : ControllerBase
{
    private readonly EventService eventService = eventService;

    [HttpGet("create")]
    public async Task<IActionResult> Get()
    {
        var newEvent = new Event
        {
            Title = "Team Meeting",
            Description = "Monthly team sync-up to discuss project progress.",
            StartTime = new DateTimeOffset(2025, 6, 19, 14, 0, 0, TimeSpan.Zero),
            EndTime = new DateTimeOffset(2025, 6, 19, 15, 0, 0, TimeSpan.Zero),
        };

        var ev = await eventService.CreateEventAsync(newEvent, "6847c7de755aeed467abdfd6");
        return new OkObjectResult(new EventDto(ev));
    }

    [HttpGet("retrieve/{hash}")]
    public async Task<IActionResult> GetAsync(string hash)
    {
        var ev = await eventService.RetrieveEventById(hash);
        return new OkObjectResult(new EventDto(ev));
    }
}
