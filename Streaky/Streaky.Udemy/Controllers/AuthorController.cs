using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Entities;
using Streaky.Udemy.Services;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IService service;
    private readonly ServiceTransient serviceTransient;
    private readonly ServiceScoped serviceScoped;
    private readonly ServiceSingleton serviceSingleton;
    private readonly ILogger<AuthorController> logger;

    public AuthorController(ApplicationDbContext context, IService service, ServiceTransient serviceTransient, ServiceScoped serviceScoped, ServiceSingleton serviceSingleton, ILogger<AuthorController> logger)
    {
        this.context = context;
        this.service = service;
        this.serviceTransient = serviceTransient;
        this.serviceScoped = serviceScoped;
        this.serviceSingleton = serviceSingleton;
        this.logger = logger;
    }

    [HttpGet("Guid")]
    public ActionResult GetGuids()
    {
        return Ok(new
        {
            AuthorControllerTransient = serviceTransient.Guid,
            ServiceA_Transient = service.GetTransient(),
            AuthorControllerScoped = serviceScoped.Guid,
            ServiceA_Scoped = service.GetScoped(),
            AuthorControllerSingleton = serviceSingleton.Guid,
            ServiceA_Singleton = service.GetSingleton()
        });
    }

    [HttpGet]
    [HttpGet("list")]
    [HttpGet("/list")]
    public async Task<ActionResult<List<Author>>> Get()
    {
        logger.LogInformation("Estamos obteniendo informacion");
        logger.LogWarning("Estamos obteniendo informacion - warning");
        service.DoTask();
        return await context.Author.Include(x => x.Books).ToListAsync();
    }

    [HttpGet("first")] // api/author/first?name=jared&surname=leon
    public async Task<ActionResult<Author>> FirstAuthor([FromHeader] int someValue, [FromQuery] string name)
    {
        return await context.Author.FirstOrDefaultAsync();
    }

    [HttpGet("second")]
    public ActionResult<Author> SecondAuthor()
    {
        return new Author() { Name = "New" };
    }

    [HttpGet("{id:int}/{param2?}")] //para valor x defecto igualar el param2 = defectovalor
    public async Task<ActionResult<Author>> Get(int id, string param2)
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

