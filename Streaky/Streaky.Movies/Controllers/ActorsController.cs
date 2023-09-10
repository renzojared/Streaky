using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;
using Streaky.Movies.Helper;
using Streaky.Movies.Services;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IStorageFiles storageFiles;
    private readonly string container = "actors";

    public ActorsController(ApplicationDbContext context, IMapper mapper, IStorageFiles storageFiles)
    {
        this.context = context;
        this.mapper = mapper;
        this.storageFiles = storageFiles;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
    {
        var queryable = context.Actors.AsQueryable();
        await HttpContext.InsertParametersPagination(queryable, paginationDTO.QuantityRecordByPage);

        var entities = await queryable.Paginate(paginationDTO).ToListAsync();

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

        if (actorCreationDTO.Photo != null)
        {
            using (var s = new MemoryStream())
            {
                await actorCreationDTO.Photo.CopyToAsync(s);
                var content = s.ToArray();
                var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                entity.Photo = await storageFiles.SaveFile(content, extension, container, actorCreationDTO.Photo.ContentType);
            }
        }

        context.Add(entity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<ActorDTO>(entity);

        return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
    {
        var actorDb = await context.Actors.FirstOrDefaultAsync(s => s.Id == id);

        if (actorDb is null)
            return NotFound();

        actorDb = mapper.Map(actorCreationDTO, actorDb); //Para actualizar solo los campos cambiados

        if (actorCreationDTO.Photo != null)
        {
            using (var s = new MemoryStream())
            {
                await actorCreationDTO.Photo.CopyToAsync(s);
                var content = s.ToArray();
                var extension = Path.GetExtension(actorCreationDTO.Photo.FileName);
                actorDb.Photo = await storageFiles.EditFile(content, extension, container, actorDb.Photo, actorCreationDTO.Photo.ContentType);
            }
        }

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument) //Para actualizaciones parciales
    {
        if (patchDocument is null)
            return BadRequest();

        var entityDb = await context.Actors.FirstOrDefaultAsync(s => s.Id == id);

        if (entityDb is null)
            return NotFound();

        var entityDto = mapper.Map<ActorPatchDTO>(entityDb);
        patchDocument.ApplyTo(entityDto, ModelState);

        var isValid = TryValidateModel(entityDto);

        if (!isValid)
            return BadRequest(ModelState);

        mapper.Map(entityDto, entityDb);

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

