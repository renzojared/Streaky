namespace Streaky.Udemy.Middlewares;

public static class LogResponseHttpMiddlewareExtensions
{
    public static IApplicationBuilder UseLogResponseHttp(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogResponseHttpMiddleware>();
    }
}

public class LogResponseHttpMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LogResponseHttpMiddleware> logger;

    public LogResponseHttpMiddleware(RequestDelegate next, ILogger<LogResponseHttpMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var ms = new MemoryStream())
        {
            var originalBody = context.Response.Body;
            context.Response.Body = ms;

            await next(context);

            ms.Seek(0, SeekOrigin.Begin);
            string response = new StreamReader(ms).ReadToEnd();
            ms.Seek(0, SeekOrigin.Begin);

            await ms.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            logger.LogInformation(response);
        }
    }
}

