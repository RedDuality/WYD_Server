using System.Net;
using System.Text.Json;
using Core.Services.Util;
using Microsoft.AspNetCore.Mvc;

//Exception Specific libraries
/*
using Microsoft.AspNetCore.Http; // Ensure this is present for HttpContext, RequestDelegate
using Microsoft.Extensions.Logging; // Ensure this is present for ILogger
using System; // Ensure this is present for Exception
using Microsoft.IdentityModel.Tokens; // Add this for SecurityTokenValidationException
using System.Collections.Generic; // Add this for KeyNotFoundException
using System.Threading; // Add this for ThreadInterruptedException
*/

namespace server.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        IActionResult errorResult = ExceptionService.GetErrorResult(exception);

        context.Response.ContentType = "application/json";

        switch (errorResult)
        {
            case ObjectResult objectResult:
                context.Response.StatusCode = objectResult.StatusCode ?? (int)HttpStatusCode.InternalServerError;
                if (objectResult.Value != null)
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(objectResult.Value));
                }
                break;
            case StatusCodeResult statusCodeResult:
                context.Response.StatusCode = statusCodeResult.StatusCode;
                break;
            default:
                // Fallback for unexpected IActionResult types
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An unexpected error occurred." }));
                break;
        }
    }
}

