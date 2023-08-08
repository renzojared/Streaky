using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    public readonly ApplicationDbContext context;

    public AuthorController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> Get()
    {
        return await context.Author.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Author author)
    {
        context.Add(author);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")] // api/authors/#number
    public async Task<ActionResult> Put(Author author, int id)
    {
        var exists = await context.Author.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        if (author.Id != id)
            return BadRequest("El id del autor no coincide con el id de la URL.");

        context.Update(author);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Author.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Author() { Id = id });
        await context.SaveChangesAsync();

        return Ok();
    }
}

