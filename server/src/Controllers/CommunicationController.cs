using Core.Services.Users;
using Core.DTO.UserAPI;
using Core.Components.ServerSentMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("Communication")]
public class CommunicationController(
    DeviceService deviceService,
    WebConnectionService webConnectionService) : ControllerBase
{
    [Authorize]
    [HttpPost("StoreFcmToken")]
    public async Task<IActionResult> StoreFcmToken(StoreFcmTokenRequestDto storeTokenDto)
    {
        await deviceService.AddDevice(storeTokenDto);
        return new OkObjectResult("");
    }

    [Authorize]
    [HttpPost("RemoveFcmToken")]
    public async Task<IActionResult> RemoveFcmToken([FromBody] string fcmToken)
    {
        await deviceService.RemoveDevice(fcmToken);
        return new OkObjectResult("");
    }

    [Authorize]
    [HttpGet("CreateSseChannel")]
    public Task CreateSseChannel()
    {
        return webConnectionService.CreateChannel(new AspNetSseResponseWriter(Response), HttpContext.RequestAborted);
    }

    /* Azure FaaS

     [Function("CreateSseChannel")]
    public async Task<HttpResponseData> CreateSseChannel(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Communication/CreateSseChannel")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var response = req.CreateResponse();
        var writer = new AzureFunctionSseResponseWriter(response);

        await _webConnectionService.CreateChannel(writer, cancellationToken);

        return response;
    }
    */
}
