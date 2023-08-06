using Microsoft.AspNetCore.Mvc;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Author>> Get()
    {
        return new List<Author>()
        {
            new Author() {Id = 1, Name = "Renzo"},
            new Author() {Id = 2, Name = "Karina"}
        };
    }
}

