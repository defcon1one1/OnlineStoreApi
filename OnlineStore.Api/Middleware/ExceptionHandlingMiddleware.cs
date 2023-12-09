using FluentValidation;
using System.Text.Json;

namespace OnlineStore.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            IEnumerable<string> errorMessages = ex.Errors.Select(error => error.ErrorMessage);

            string jsonResponse = JsonSerializer.Serialize(new
            {
                Errors = errorMessages
            });
            await context.Response.WriteAsync(jsonResponse);
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var exceptionInfo = new
            {
                Message = "An error occurred while processing your request."
            };

            string jsonResponse = JsonSerializer.Serialize(exceptionInfo);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}