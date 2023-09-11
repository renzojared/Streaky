using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;
using Streaky.Movies.Helper;

namespace Streaky.Movies.Controllers;

public class CustomBaseController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public CustomBaseController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
    {
        var entities = await context.Set<TEntity>().AsNoTracking().ToListAsync();
        var dtos = mapper.Map<List<TDTO>>(entities);
        return dtos;
    }

    protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO) where TEntity : class
    {
        var queryable = context.Set<TEntity>().AsQueryable();
        await HttpContext.InsertParametersPagination(queryable, paginationDTO.QuantityRecordByPage);

        var entities = await queryable.Paginate(paginationDTO).ToListAsync();

        return mapper.Map<List<TDTO>>(entities);
    }

    protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
    {
        var entity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        if (entity is null)
            return NotFound();

        var dto = mapper.Map<TDTO>(entity);
        return dto;
    }

    protected async Task<ActionResult> Post<TCreation, TEntity, TRead>(TCreation creationDTO, string nameRoute) where TEntity : class, IId
    {
        var entity = mapper.Map<TEntity>(creationDTO);
        context.Add(entity);

        await context.SaveChangesAsync();

        var dtoRead = mapper.Map<TRead>(entity);

        return new CreatedAtRouteResult(nameRoute, new { id = entity.Id }, dtoRead);
    }

    protected async Task<ActionResult> Put<TCreation, TEntity>(int id, TCreation creationDTO) where TEntity : class, IId
    {
        var entity = mapper.Map<TEntity>(creationDTO);
        entity.Id = id;
        context.Entry(entity).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    protected async Task<ActionResult> Delete<TEntity> (int id) where TEntity : class, IId, new()
    {
        var exists = await context.Set<TEntity>().AnyAsync(s => s.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new TEntity() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }

    protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)
        where TDTO : class
        where TEntity : class, IId
    {
        if (patchDocument is null)
            return BadRequest();

        var entityDb = await context.Set<TEntity>().FirstOrDefaultAsync(s => s.Id == id);

        if (entityDb is null)
            return NotFound();

        var entityDto = mapper.Map<TDTO>(entityDb);
        patchDocument.ApplyTo(entityDto, ModelState);

        var isValid = TryValidateModel(entityDto);

        if (!isValid)
            return BadRequest(ModelState);

        mapper.Map(entityDto, entityDb);

        await context.SaveChangesAsync();

        return NoContent();
    }
}

