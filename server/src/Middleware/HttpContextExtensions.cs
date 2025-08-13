using Core.Model;

namespace server.Middleware;

public static class HttpContextExtensions
{
    public static User GetUser(this HttpContext httpContext)
    {
        // Use a pattern matching statement to safely retrieve and cast the user object.
        // The 'as' keyword will return null if the object is not of type 'User'.
        if (httpContext.Items.TryGetValue("User", out var userObject) && userObject is User user)
        {
            return user;
        }
        throw new ArgumentNullException("User not found");
    }
}