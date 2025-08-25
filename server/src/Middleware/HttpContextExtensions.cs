using Core.Model;

namespace server.Middleware;

public static class HttpContextExtensions
{
    public static User GetUser(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue("User", out var userObject) && userObject is User user)
        {
            return user;
        }
        throw new ArgumentNullException("User not found");
    }
}