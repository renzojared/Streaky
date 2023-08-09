using Microsoft.AspNetCore.Mvc.Filters;

namespace Streaky.Udemy.Filters;

public class ExceptionFiler : ExceptionFilterAttribute
{
    public ExceptionFiler(ILogger<ExceptionFiler> logger)
    {
        Logger = logger;
    }

    public ILogger<ExceptionFiler> Logger { get; }

    public override void OnException(ExceptionContext context)
    {
        Logger.LogError(context.Exception, context.Exception.Message);

        base.OnException(context);
    }
}

