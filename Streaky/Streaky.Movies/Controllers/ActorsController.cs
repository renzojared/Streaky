using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public ActorsController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActorDTO>>> Get()
    {
        var entities = await context.Actors.ToListAsync();

        return mapper.Map<List<ActorDTO>>(entities);
    }

    [HttpGet("{id:int}", Name = "getActor")]
    public async Task<ActionResult<ActorDTO>> Get(int id)
    {
        var entity = await context.Actors.FirstOrDefaultAsync(s => s.Id == id);

        if (entity is null)
            return NotFound();

        return mapper.Map<ActorDTO>(entity);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
    {
        var entity = mapper.Map<Actor>(actorCreationDTO);
        context.Add(entity);
        //await context.SaveChangesAsync();

        var dto = mapper.Map<ActorDTO>(entity);

        return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
    {
        var entity = mapper.Map<Actor>(actorCreationDTO);
        entity.Id = id;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Actors.AnyAsync(s => s.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Actor() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }
}

