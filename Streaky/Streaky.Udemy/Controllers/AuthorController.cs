using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public AuthorController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> Get()
    {
        return await context.Author.ToListAsync();
    }

    [HttpGet("{id:int}")] //para valor x defecto igualar el param2 = defectovalor
    public async Task<ActionResult<Author>> Get(int id)
    {
        var author = await context.Author.FirstOrDefaultAsync(x => x.Id == id);

        if (author == null)
            return NotFound();

        return author;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Author>> Get([FromRoute] string name)
    {
        var author = await context.Author.FirstOrDefaultAsync(x => x.Name.Contains(name));

        if (author == null)
            return NotFound();

        return author;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Author author)
    {
        var existsAuthorWithSameName = await context.Author.AnyAsync(x => x.Name == author.Name);

        if (existsAuthorWithSameName)
            return BadRequest($"Ya existe un autor con el nombre {author.Name}");

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

