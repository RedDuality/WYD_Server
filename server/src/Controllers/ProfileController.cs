using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Profiles;
using Core.DTO.ProfileAPI;

using server.Middleware;


namespace server.Controllers;

[ApiController]
[Route("Profile")]
public class ProfileController(ContextManager contextManager, ProfileService profileService, ProfileTagService profileTagService) : ControllerBase
{

    [HttpGet("SearchBytag/{searchTag}")]
    public async Task<IActionResult> SearchBytag(string searchTag)
    {
        // Admin
        var profileDtos = await profileTagService.SearchByTagAsync(searchTag);
        return new OkObjectResult(profileDtos);
    }

    [Authorize]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequestDto updateDto)
    {
        // Admin
        var user = await contextManager.GetCurrentUser();
        var resultDto = await profileService.Update(user.Id, updateDto);
        return new OkObjectResult(resultDto);
    }

    [HttpPost("Retrieve")]
    public async Task<IActionResult> Retrieve([FromBody] HashSet<string> profileIds)
    {
        var profile = await profileService.RetrieveMultipleProfileById(profileIds);
        return new OkObjectResult(profile);
    }

    [HttpGet("RetrieveDetailed/{profileId}")]
    public async Task<IActionResult> RetrieveDetailed(string profileId)
    {
        // Viewer
        var user = await contextManager.GetCurrentUser();
        var profile = await profileService.RetrieveDetailedProfileById(user, profileId);
        return new OkObjectResult(profile);
    }
}



