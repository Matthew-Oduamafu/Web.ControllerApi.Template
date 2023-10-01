using System.Net;
using Web.ControllerApi.Template.Models;

namespace Web.ControllerApi.Template.CustomMiddlewares;

public class ExceptionMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {ExMessage}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ExceptionDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error.",
            Details = exception.ToString()
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}