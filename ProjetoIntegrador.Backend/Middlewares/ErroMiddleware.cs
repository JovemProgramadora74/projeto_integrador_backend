using System.Net;
using System.Text.Json;

namespace ProjetoIntegrador.Backend.Middlewares;

public class ErroMiddleware(RequestDelegate next, ILogger<ErroMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro inesperado.");
            await TratarExececaoAsync(context, ex);
        }
    }

    private static Task TratarExececaoAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var resultado = JsonSerializer.Serialize(new { message = exception.Message });

        return context.Response.WriteAsync(resultado);
    }
}