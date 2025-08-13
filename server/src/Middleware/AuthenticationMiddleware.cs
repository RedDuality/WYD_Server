using Core.Services.Util;

namespace server.Middleware;

public class AuthenticationMiddleware(RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context, TokenService tokenService)
    {
        // No try...catch block here. We let exceptions bubble up to the ExceptionHandlingMiddleware.
        var user = await tokenService.VerifyRequestAsync(context.Request);
        context.Items["User"] = user;
        
        await next(context);
    }
}
