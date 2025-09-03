using System.Security.Claims;
using Core.Model;
using Core.Services.Util;
using Microsoft.Extensions.Primitives;

namespace server.Middleware;

public class ContextManager(IHttpContextAccessor httpContextAccessor, ContextService contextService)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<User> GetCurrentUser()
    {
        // Get the ClaimsPrincipal from the accessor
        var user = _httpContextAccessor.HttpContext?.User;
        return await contextService.GetUser(user);
    }

    public string GetCurrentProfileId()
    {
        return ContextService.RetrieveFromHeaders(_httpContextAccessor.HttpContext!.Request, "Current-Profile");
    }
}