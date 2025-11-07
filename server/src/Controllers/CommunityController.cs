using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Communities;
using Core.DTO.CommunityAPI;

using server.Middleware;


namespace server.Controllers;

[ApiController]
[Route("Community")]
public class CommunityController(ContextManager contextManager, CommunityService communityService) : ControllerBase
{

    [HttpGet("Retrieve")]
    public async Task<IActionResult> Retrieve()
    {
        // Viewer
        var profile = await contextManager.GetCurrentProfile();
        var communities = await communityService.Retrieve(profile);
        return new OkObjectResult(communities);
    }

    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateCommunityRequestDto createDto)
    {
        // Admin
        var profile = await contextManager.GetCurrentProfile();
        var newCommunity = await communityService.Create(createDto, profile);
        return new OkObjectResult(newCommunity);
    }

}



