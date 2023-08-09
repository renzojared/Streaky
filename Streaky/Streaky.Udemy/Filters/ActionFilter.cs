using Microsoft.AspNetCore.Mvc.Filters;

namespace Streaky.Udemy.Filters;

public class ActionFilter : IActionFilter
{
    public ILogger<ActionFilter> logger { get; }

    public ActionFilter(ILogger<ActionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("Antes de ejecutar la accion.");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation("Despues de ejecutar la accion.");
    }
}

