using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;
using Streaky.Movies.Helper;
using Streaky.Movies.Services;
using System.Linq.Dynamic.Core;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : CustomBaseController
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IStorageFiles storageFiles;
    private readonly string container = "movies";
    private readonly ILogger<MoviesController> logger;

    public MoviesController(ApplicationDbContext context, IMapper mapper, IStorageFiles storageFiles, ILogger<MoviesController> logger) : base(context, mapper)
    {
        this.context = context;
        this.mapper = mapper;
        this.storageFiles = storageFiles;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<MoviesIndexDTO>> Get()
    {
        var top = 5;
        var today = DateTime.Today;

        var nextReleases = await context.Movies
            .Where(s => s.ReleaseDate > today)
            .OrderBy(s => s.ReleaseDate)
            .Take(top)
            .ToListAsync();

        var onCinema = await context.Movies
            .Where(s => s.InCinema)
            .Take(top)
            .ToListAsync();

        var result = new MoviesIndexDTO();
        result.NextReleases = mapper.Map<List<MovieDTO>>(nextReleases);
        result.InCinema = mapper.Map<List<MovieDTO>>(onCinema);

        return result;
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMoviesDTO filterMoviesDTO)
    {
        var movieQueryable = context.Movies.AsQueryable();

        if (!string.IsNullOrEmpty(filterMoviesDTO.Title))
            movieQueryable = movieQueryable.Where(s => s.Title.Contains(filterMoviesDTO.Title));

        if (filterMoviesDTO.InCinema)
            movieQueryable = movieQueryable.Where(s => s.InCinema);

        if (filterMoviesDTO.NextReleases)
        {
            var today = DateTime.Today;
            movieQueryable = movieQueryable.Where(s => s.ReleaseDate > today);
        }

        if (filterMoviesDTO.GenderId != 0)
            movieQueryable = movieQueryable
                .Where(s => s.MoviesGenders.Select(y => y.GenderId)
                .Contains(filterMoviesDTO.GenderId));

        if (!string.IsNullOrEmpty(filterMoviesDTO.FieldOrder))
        {
            var typeOrder = filterMoviesDTO.OrderAscending ? "ascending" : "descending";
            try
            {
                movieQueryable = movieQueryable.OrderBy($"{filterMoviesDTO.FieldOrder} {typeOrder}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
            
        }

        await HttpContext.InsertParametersPagination(movieQueryable, filterMoviesDTO.QuantityRecordsByPage);

        var movies = await movieQueryable.Paginate(filterMoviesDTO.Pagination).ToListAsync();

        return mapper.Map<List<MovieDTO>>(movies);
    }

    [HttpGet("{id:int}", Name = "getMovie")]
    public async Task<ActionResult<MovieDetailDTO>> Get(int id)
    {
        var movie = await context.Movies
            .Include(s => s.MoviesActors)
            .ThenInclude(s => s.Actor)
            .Include(s => s.MoviesGenders)
            .ThenInclude(s => s.Gender)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (movie is null)
            return NotFound();

        movie.MoviesActors = movie.MoviesActors.OrderBy(s => s.Order).ToList();

        return mapper.Map<MovieDetailDTO>(movie);
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
        return await Patch<Movie, MoviePatchDTO>(id, patchDocument);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        return await Delete<Movie>(id);
    }
}

