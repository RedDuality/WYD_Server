using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Profiles;
using Core.DTO.ProfileAPI;

using server.Middleware;
using Core.Services.Users;


namespace server.Controllers;

[ApiController]
[Route("Profile")]
public class ProfileController(ContextManager contextManager, UserService userService, ProfileService profileService, ProfileTagService profileTagService) : ControllerBase
{
    [Authorize]
    [HttpGet("SearchBytag/{searchTag}")]
    public async Task<IActionResult> SearchBytag(string searchTag)
    {
        var profileDtos = await profileTagService.SearchByTagAsync(searchTag);
        return new OkObjectResult(profileDtos);
    }

    [Authorize]
    [HttpPost("Retrieve")]
    public async Task<IActionResult> Retrieve([FromBody] HashSet<string> profileIds)
    {
        var profile = await profileService.RetrieveMultipleProfileById(profileIds);
        return new OkObjectResult(profile);
    }

    [Authorize(policy:"CanViewProfileDetails")]
    [HttpGet("RetrieveDetailed/{profileId}")]
    public async Task<IActionResult> RetrieveDetailed(string profileId)
    {
        var accountId = contextManager.GetAccountId();
        var userId = contextManager.GetUserId();

        var currentUser = await userService.Retrieve(userId, accountId);
        var profile = await profileService.RetrieveDetailedProfileById(currentUser, profileId);
        return new OkObjectResult(profile);
    }
    

    [Authorize(policy: "CanEditCommunity")]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequestDto updateDto)
    {
        var accountId = contextManager.GetAccountId();
        var userId = contextManager.GetUserId();

        var currentUser = await userService.Retrieve(userId, accountId);
        var resultDto = await profileService.Update(currentUser.Id, updateDto);
        return new OkObjectResult(resultDto);
    }


}



