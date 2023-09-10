using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GendersController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public GendersController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenderDTO>>> Get()
    {
        var entities = await context.Genders.ToListAsync();
        var dtos = mapper.Map<List<GenderDTO>>(entities);
        return dtos;
    }

    [HttpGet("{id:int}", Name = "getGender")]
    public async Task<ActionResult<GenderDTO>> Get(int id)
    {
        var entity = await context.Genders.FirstOrDefaultAsync(s => s.Id == id);

        if (entity is null)
            return NotFound();

        var dto = mapper.Map<GenderDTO>(entity);
        return dto;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] GenderCreationDTO genderCreationDTO)
    {
        var entity = mapper.Map<Gender>(genderCreationDTO);
        context.Add(entity);

        await context.SaveChangesAsync();

        var genderDto = mapper.Map<GenderDTO>(entity);

        return new CreatedAtRouteResult("getGender", new { id = genderDto.Id }, genderDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] GenderCreationDTO genderCreationDTO)
    {
        var entity = mapper.Map<Gender>(genderCreationDTO);
        entity.Id = id;
        context.Entry(entity).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Genders.AnyAsync(s => s.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Gender() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }
}

