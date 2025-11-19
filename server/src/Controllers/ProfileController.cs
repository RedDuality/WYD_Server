using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Core.Services.Profiles;
using Core.DTO.ProfileAPI;

using Core.Services.Users;


namespace server.Controllers;

[ApiController]
[Route("Profile")]
public class ProfileController(ProfileService profileService, ProfileTagService profileTagService) : ControllerBase
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


}



