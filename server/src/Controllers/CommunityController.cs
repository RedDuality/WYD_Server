using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Communities;
using Core.DTO.CommunityAPI;

using Core.Services.Profiles;
using Core.Services.Util;


namespace server.Controllers;

[ApiController]
[Route("Community")]
public class CommunityController(IContextManager contextManager, ProfileService profileService, CommunityService communityService) : ControllerBase
{
    [Authorize(policy:"CanViewCommunity")]
    [HttpGet("Retrieve")]
    public async Task<IActionResult> RetrieveCommunities()
    {
        // Viewer
        var profileId = contextManager.GetCurrentProfileId();
        var profile =  await profileService.RetrieveProfileById(profileId);
        var communities = await communityService.RetrieveCommunities(profile);
        return new OkObjectResult(communities);
    }

    [Authorize(policy:"CanCreateCommunity")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateCommunityRequestDto createDto)
    {
        // Admin
        var profileId = contextManager.GetCurrentProfileId();
        var profile =  await profileService.RetrieveProfileById(profileId);
        var newCommunity = await communityService.Create(createDto, profile);
        return new OkObjectResult(newCommunity);
    }

}



