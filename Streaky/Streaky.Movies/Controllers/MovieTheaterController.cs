using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Controllers;

[Route("api/[controller]")]
public class MovieTheaterController : CustomBaseController
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly GeometryFactory geometryFactory;

    public MovieTheaterController(ApplicationDbContext context, IMapper mapper, GeometryFactory geometryFactory) : base(context, mapper)
    {
        this.context = context;
        this.mapper = mapper;
        this.geometryFactory = geometryFactory;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieTheaterDTO>>> Get()
    {
        return await Get<MovieTheater, MovieTheaterDTO>();
    }

    [HttpGet("{id:int}", Name = "getMovieTheater")]
    public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
    {
        return await Get<MovieTheater, MovieTheaterDTO>(id);
    }

    [HttpGet("Nearby")]
    public async Task<ActionResult<List<MovieTheaterNearbyDTO>>> Nearby([FromQuery] MovieTheaterNearbyFilterDTO filter)
    {
        var locationUser = geometryFactory.CreatePoint(new Coordinate(filter.Length, filter.Latitude));
        var movieTheater = await context.MovieTheaters
            .OrderBy(s => s.Location.Distance(locationUser))
            .Where(s => s.Location.IsWithinDistance(locationUser, filter.DistanceinKms * 1000))
            .Select(s => new MovieTheaterNearbyDTO
            {
                Id = s.Id,
                Name = s.Name,
                Latitude = s.Location.Y,
                Length = s.Location.X,
                DistanceInMeters = Math.Round(s.Location.Distance(locationUser))
            }).ToListAsync();

        return movieTheater;
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
    {
        return await Post<MovieTheaterCreationDTO, MovieTheater, MovieTheaterDTO>(movieTheaterCreationDTO, "getMovieTheater");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
    {
        return await Put<MovieTheaterCreationDTO, MovieTheater>(id, movieTheaterCreationDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        return await Delete<MovieTheater>(id);
    }
}
