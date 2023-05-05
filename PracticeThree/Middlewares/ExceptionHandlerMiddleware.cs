using System.Globalization;

namespace UPB.PracticeThree.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //Encapsuladno al siguiente
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        catch (System.Exception exception)
        {
            //Log ex.Message
            HandleException(context, exception);
        }
    }

    private static Task HandleException(HttpContext context,  Exception exception)
    {
        context.Response.ContentType = "text/json";
        //context.Response.StatusCode = 500;
        return context.Response.WriteAsync("Sucedio el sgte error: " + exception.Message);
    }
}

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}