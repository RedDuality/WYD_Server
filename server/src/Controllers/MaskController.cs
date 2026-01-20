using Microsoft.AspNetCore.Mvc;
using Core.DTO.MaskAPI;
using Core.Services.Util;
using Core.Services.Masks;


namespace server.Controllers;

[ApiController]
[Route("Mask")]
public class MaskController(IContextManager contextManager, MaskService maskService) : ControllerBase
{

    //[Authorize(policy: "CanCreateMasks")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateMask([FromBody] CreateMaskRequestDto createMaskDto)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await maskService.CreateMaskAsync(profileId, createMaskDto);
        return new OkObjectResult(mask);
    }

    //[Authorize(policy: "CanCreateMasks")]
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateMask([FromBody] UpdateMaskRequestDto updateMaskDto)
    {
        var mask = await maskService.UpdateMaskAsync(updateMaskDto);
        return new OkObjectResult(mask);
    }

    [HttpPost("getMasks")]
    public async Task<IActionResult> GetMasks([FromBody] RetrieveMultipleMaskRequestDto retrieveMasksDto)
    {
        var masks = await maskService.RetrieveMasks(retrieveMasksDto);
        return new OkObjectResult(masks);
    }


    [HttpGet("retrieveEventMask/{eventId}")]
    public async Task<IActionResult> RetrieveEventMask(string eventId)
    {
        var profileId = contextManager.GetCurrentProfileId();
        var mask = await maskService.RetrieveEventMask(eventId, profileId);
        return new OkObjectResult(mask);
    }

    [HttpPost("ListByProfile")]
    public async Task<IActionResult> ListByProfile([FromBody] RetrieveMultipleMaskRequestDto retrieveDto)
    {
        var masks = await maskService.RetrieveMasks(retrieveDto);

        return new OkObjectResult(masks);
    }
}
