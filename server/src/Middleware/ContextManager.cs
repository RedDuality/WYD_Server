using Core.Services.Util;

namespace server.Middleware;

public class ContextManager(IHttpContextAccessor httpContextAccessor) : IContextManager
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetAccountId()
    {
        var userPrincipal = _httpContextAccessor.HttpContext?.User;
        return ContextService.GetAccountId(userPrincipal);
    }

    public string GetEmail()
    {
        var userPrincipal = _httpContextAccessor.HttpContext?.User;
        return ContextService.GetEmail(userPrincipal);
    }


    public string GetUserId()
    {
        var userPrincipal = _httpContextAccessor.HttpContext?.User;
        return ContextService.GetUserId(userPrincipal);
    }


    public string GetCurrentProfileId()
    {
        return ContextService.RetrieveFromHeaders(_httpContextAccessor.HttpContext!.Request, "Current-Profile");
    }
}