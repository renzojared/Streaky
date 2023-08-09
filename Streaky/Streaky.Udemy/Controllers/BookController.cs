using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    public readonly ApplicationDbContext context;

    public BookController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        return await context.Book.FirstOrDefaultAsync(x => x.Id == id);
    }

    /*[HttpPost]
    public async Task<ActionResult> Post(Book book)
    {
        var existsAuthor = await context.Author.AnyAsync(x => x.Id == book.AuthorId);

        if (!existsAuthor)
            return BadRequest($"No existe el author de Id: {book.AuthorId}");

        context.Add(book);

        await context.SaveChangesAsync();

        return Ok();
    }*/
}

