using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> log)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try { await next(ctx); }
        catch (InvalidOperationException ex)
        {
            log.LogWarning(ex, "Business rule violation");
            ctx.Response.StatusCode = 409;
            ctx.Response.ContentType = "application/json";
            var problem = new ProblemDetails { Status = 409, Title = "Conflict", Detail = ex.Message };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unhandled exception");
            ctx.Response.StatusCode = 500;
            ctx.Response.ContentType = "application/json";
            var problem = new ProblemDetails { Status = 500, Title = "Internal Server Error", Detail = ex.Message };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
