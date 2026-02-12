using Microsoft.AspNetCore.Mvc;
using Core.DTO.MaskAPI;
using Core.Services.Util;
using Core.Services.Masks;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers;

[ApiController]
[Route("Mask")]
public class MaskController(
    IContextManager contextManager,
    MaskService maskService,
    EventMaskService eventMaskService) : ControllerBase
{

    [Authorize(policy: "CanCreateMasks")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateMask([FromBody] CreateMaskRequestDto createMaskDto)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await maskService.CreateMaskAsync(profileId, createMaskDto);
        return new OkObjectResult(mask);
    }

    [Authorize(policy: "CanEditMasks")]
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateMask([FromBody] UpdateMaskRequestDto updateMaskDto)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await maskService.UpdateMaskAsync(profileId, updateMaskDto);
        return new OkObjectResult(mask);
    }

    [Authorize(policy: "CanDeleteMasks")]
    [HttpGet("Delete/{maskId}")]
    public async Task<IActionResult> DeleteMask(string maskId)
    {
        var profileId = contextManager.GetCurrentProfileId();
        await maskService.DeleteMaskAsync(profileId, maskId);
        return new OkObjectResult("");
    }

    [Authorize(policy: "CanReadEvents")]
    [HttpGet("retrieveEventMask/{eventId}")]
    public async Task<IActionResult> RetrieveEventMask(string eventId)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await eventMaskService.RetrieveEventMask(eventId, profileId);
        return new OkObjectResult(mask);
    }

    [HttpPost("RetrieveUserMasks")]
    public async Task<IActionResult> RetrieveUserMasks([FromBody] RetrieveUserMaskRequestDto retrieveDto)
    {
        var userId = contextManager.GetUserId();
        var masks = await maskService.RetrieveUserMasks(userId, retrieveDto);

        return new OkObjectResult(masks);
    }

    [HttpPost("RetrieveUpdated")]
    public async Task<IActionResult> RetrieveUpdated([FromBody] RetrieveUserMaskRequestDto retrieveDto)
    {
        var userId = contextManager.GetUserId();
        var masks = await maskService.RetrieveUpdated(userId, retrieveDto);

        return new OkObjectResult(masks);
    }

    [HttpPost("RetrieveProfileMasks")]
    public async Task<IActionResult> RetrieveProfileMasks([FromBody] RetrieveProfileMaskRequestDto retrieveDto)
    {
        var masks = await maskService.RetrieveProfileMasks(retrieveDto);

        return new OkObjectResult(masks);
    }

    [HttpGet("retrieveMask/{maskId}")]
    public async Task<IActionResult> RetrieveMask(string maskId)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await maskService.RetrieveSingleMask(profileId, maskId);
        return new OkObjectResult(mask);
    }

}
