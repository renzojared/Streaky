using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streaky.Udemy.DTOs;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RouteController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;

    public RouteController(IAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpGet(Name = "GetRoot")] //importante, sino no devuelve informacion en url.link
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<DataHATEOAS>>> Get()
    {
        var dataHateoas = new List<DataHATEOAS>();

        var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

        dataHateoas.Add(new DataHATEOAS(link: Url.Link("GetRoot", new { }), description: "self", method: "GET"));

        dataHateoas.Add(new DataHATEOAS(link: Url.Link("getAuthors", new { }), description: "authors", method: "GET"));

        if (isAdmin.Succeeded)
        {
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("createAuthor", new { }), description: "author-create", method: "POST"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("createBook", new { }), description: "book-create", method: "POST"));
        }

        return dataHateoas;
    }
}

