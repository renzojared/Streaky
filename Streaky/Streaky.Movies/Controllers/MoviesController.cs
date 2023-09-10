using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;
using Streaky.Movies.Services;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IStorageFiles storageFiles;
    private readonly string container = "movies";

    public MoviesController(ApplicationDbContext context, IMapper mapper, IStorageFiles storageFiles)
    {
        this.context = context;
        this.mapper = mapper;
        this.storageFiles = storageFiles;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDTO>>> Get()
    {
        var movie = await context.Movies.ToListAsync();
        return mapper.Map<List<MovieDTO>>(movie);
    }

    [HttpGet("{id:int}", Name = "getMovie")]
    public async Task<ActionResult<MovieDTO>> Get(int id)
    {
        var movie = await context.Movies.FirstOrDefaultAsync(s => s.Id == id);

        if (movie is null)
            return NotFound();

        return mapper.Map<MovieDTO>(movie);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
    {
        var movie = mapper.Map<Movie>(movieCreationDTO);

        if (movieCreationDTO.Poster != null)
        {
            using (var s = new MemoryStream())
            {
                await movieCreationDTO.Poster.CopyToAsync(s);
                var content = s.ToArray();
                var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                movie.Poster = await storageFiles.SaveFile(content, extension, container, movieCreationDTO.Poster.ContentType);
            }
        }

        AsignOrderMovies(movie);

        context.Add(movie);
        await context.SaveChangesAsync();
        var movieDTO = mapper.Map<MovieDTO>(movie);

        return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDTO);
    }

    private void AsignOrderMovies(Movie movie)
    {
        if (movie.MoviesActors != null)
        {
            for (int i = 0; i < movie.MoviesActors.Count(); i++)
            {
                movie.MoviesActors[i].Order = i;
            }
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
    {
        var movieDb = await context.Movies
            .Include(s => s.MoviesActors)
            .Include(s => s.MoviesGenders)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (movieDb is null)
            return NotFound();

        movieDb = mapper.Map(movieCreationDTO, movieDb); //Para actualizar solo los campos cambiados

        if (movieCreationDTO.Poster != null)
        {
            using (var s = new MemoryStream())
            {
                await movieCreationDTO.Poster.CopyToAsync(s);
                var content = s.ToArray();
                var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                movieDb.Poster = await storageFiles.EditFile(content, extension, container, movieDb.Poster, movieCreationDTO.Poster.ContentType);
            }
        }

        AsignOrderMovies(movieDb);

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument) //Para actualizaciones parciales
    {
        if (patchDocument is null)
            return BadRequest();

        var entityDb = await context.Movies.FirstOrDefaultAsync(s => s.Id == id);

        if (entityDb is null)
            return NotFound();

        var entityDto = mapper.Map<MoviePatchDTO>(entityDb);
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
        var exists = await context.Movies.AnyAsync(s => s.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Movie() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }
}

