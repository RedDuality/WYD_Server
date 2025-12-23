using Microsoft.AspNetCore.Mvc;
using Core.DTO.MaskAPI;
using Microsoft.AspNetCore.Authorization;
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
        var mask = await maskService.CreateMask(profileId, createMaskDto);
        return new OkObjectResult(mask);
    }

    [HttpPost("getMasks")]
    public async Task<IActionResult> GetMasks([FromBody] RetrieveMultipleMaskRequestDto retrieveMasksDto)
    {
        var masks = await maskService.RetrieveMasks(retrieveMasksDto);
        return new OkObjectResult(masks);
    }

    [HttpPost("ListByProfile")]
    public async Task<IActionResult> ListByProfile([FromBody] RetrieveMultipleMaskRequestDto retrieveDto)
    {
        var masks = await maskService.RetrieveMasks(retrieveDto);

        return new OkObjectResult(masks);
    }
}
