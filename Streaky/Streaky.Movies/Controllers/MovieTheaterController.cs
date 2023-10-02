using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Controllers;

[Route("api/[controller]")]
public class MovieTheaterController : CustomBaseController
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public MovieTheaterController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        this.context = context;
        this.mapper = mapper;
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
