using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Profiles;
using Core.DTO.ProfileAPI;
using Core.Services.Util;
using Core.Services.Users;
using Core.External.ImportPlatform.GoogleCalendar;

namespace server.Controllers;

[ApiController]
[Route("Profile")]
public class ProfileController(
    IContextManager contextManager,
    ProfileService profileService,
    UserService userService,
    ProfileTagService profileTagService) : ControllerBase
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

    [Authorize(policy: "CanViewProfileDetails")]
    [HttpGet("RetrieveDetailed/{profileId}")]
    public async Task<IActionResult> RetrieveDetailed(string profileId)
    {
        var profile = await profileService.RetrieveDetailedProfileById(profileId);
        return new OkObjectResult(profile);
    }


    [Authorize(policy: "CanEditProfile")]
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequestDto updateDto)
    {
        var resultDto = await profileService.Update(updateDto);
        return new OkObjectResult(resultDto);
    }

    [Authorize(policy: "CanImpersonateProfile")]
    [HttpPost("Import")]
    public async Task<IActionResult> Import([FromBody] ImportRequestDto importDto)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var userId = contextManager.GetUserId();

        var accountEmail = await GoogleCalendarService.GetEmailFromIdToken(importDto.AccessToken);
        await userService.CheckUserHaveAccount(userId, accountEmail);
        
        await profileService.Import(importDto.AccessToken, userId, profileId);
        return new OkObjectResult("");
    }



}



