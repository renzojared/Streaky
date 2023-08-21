using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Streaky.Udemy.DTOs;

namespace Streaky.Udemy.Services;

public class GenerateLinks
{
    private readonly IAuthorizationService authorizationService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IActionContextAccessor actionContextAccessor;

    public GenerateLinks(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
    {
        this.authorizationService = authorizationService;
        this.httpContextAccessor = httpContextAccessor;
        this.actionContextAccessor = actionContextAccessor;
    }

    private IUrlHelper BuildURLHelper()
    {
        var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        return factory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    private async Task<bool> IsAdmin()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var result = await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");
        return result.Succeeded;
    }

    public async Task GeneratedLinks(AuthorDTO authorDTO)
    {
        var isAdmin = await IsAdmin();
        var Url = BuildURLHelper();
        authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAuthor", new { id = authorDTO.Id }), description: "self", method: "GET"));

        if (isAdmin)
        {
            authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("updateAuthor", new { id = authorDTO.Id }), description: "author-update", method: "PUT"));
            authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("deleteAuthor", new { id = authorDTO.Id }), description: "author-delete", method: "DELETE"));
        }
    }
}

