using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public AuthorController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuthorDTO>>> Get()
    {
        var authors = await context.Author.ToListAsync();
        return mapper.Map<List<AuthorDTO>>(authors);
    }

    [HttpGet("{id:int}", Name = "getAuthor")] //para valor x defecto igualar el param2 = defectovalor
    public async Task<ActionResult<AuthorDTOWithBook>> Get(int id)
    {
        var author = await context.Author
            .Include(_ => _.AuthorBooks)
            .ThenInclude(_ => _.Book)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (author == null)
            return NotFound();

        return mapper.Map<AuthorDTOWithBook>(author);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<List<AuthorDTO>>> Get([FromRoute] string name)
    {
        var authors = await context.Author.Where(x => x.Name.Contains(name)).ToListAsync();

        if (!authors.Any())
            return NotFound();

        return mapper.Map<List<AuthorDTO>>(authors);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AuthorCreationDTO authorCreationDTO)
    {
        var existsAuthorWithSameName = await context.Author.AnyAsync(x => x.Name == authorCreationDTO.Name);

        if (existsAuthorWithSameName)
            return BadRequest($"Ya existe un autor con el nombre {authorCreationDTO.Name}");

        var author = mapper.Map<Author>(authorCreationDTO);

        context.Add(author);
        await context.SaveChangesAsync();

        var authorDTO = mapper.Map<AuthorDTO>(author);

        return CreatedAtRoute("getAuthor", new { id = author.Id }, authorDTO);
    }

    [HttpPut("{id:int}")] // api/authors/#number
    public async Task<ActionResult> Put(AuthorCreationDTO authorCreationDTO, int id)
    {
        var exists = await context.Author.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        var author = mapper.Map<Author>(authorCreationDTO);
        author.Id = id;

        context.Update(author);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Author.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Author() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }
}

