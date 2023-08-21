using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Services;

namespace Streaky.Udemy.Utilities;

public class HATEOASAuthorFilterAttribute : HATEOASFIlterAttribute
{
    private readonly GenerateLinks generateLinks;

    public HATEOASAuthorFilterAttribute(GenerateLinks generateLinks)
    {
        this.generateLinks = generateLinks;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var isInclude = ToIncludeHATEOAS(context);

        if (!isInclude)
        {
            await next();
            return;
        }
        var result = context.Result as ObjectResult;

        var authorDTO = result.Value as AuthorDTO;

        if (authorDTO == null)
        {
            var authorsDTO = result.Value as List<AuthorDTO> ?? throw new ArgumentException("Se esperaba una instancia de AuthorDTO o List<AuthorDTO>");

            authorsDTO.ForEach(async author => await generateLinks.GeneratedLinks(author));
            result.Value = authorsDTO;
        }
        else
            await generateLinks.GeneratedLinks(authorDTO);

        await next();
    }
}

