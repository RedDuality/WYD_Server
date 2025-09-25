using Core.Model.Users;
using Core.Model.Profiles;
using Core.Services.Users;
using Core.Services.Util;

namespace server.Middleware;

public class ContextManager(IHttpContextAccessor httpContextAccessor, ContextService contextService, ProfileService profileService)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<User> GetCurrentUser()
    {
        // Get the ClaimsPrincipal from the accessor
        var user = _httpContextAccessor.HttpContext?.User;
        return await contextService.GetUser(user);
    }

    public async Task<Profile> GetCurrentProfile()
    {
        // Get the ClaimsPrincipal from the accessor
        var profileId = GetCurrentProfileId();
        return await profileService.RetrieveProfileById(profileId);
    }

    public string GetCurrentProfileId()
    {
        return ContextService.RetrieveFromHeaders(_httpContextAccessor.HttpContext!.Request, "Current-Profile");
    }
}