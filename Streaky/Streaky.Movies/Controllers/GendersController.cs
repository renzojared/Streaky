using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GendersController : CustomBaseController
{
    public GendersController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<GenderDTO>>> Get()
    {
        return await Get<Gender, GenderDTO>();
    }

    [HttpGet("{id:int}", Name = "getGender")]
    public async Task<ActionResult<GenderDTO>> Get(int id)
    {
        return await Get<Gender, GenderDTO>(id);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] GenderCreationDTO genderCreationDTO)
    {
        return await Post<GenderCreationDTO, Gender, GenderDTO>(genderCreationDTO, "getGender");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] GenderCreationDTO genderCreationDTO)
    {
        return await Put<GenderCreationDTO, Gender>(id, genderCreationDTO);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        return await Delete<Gender>(id);
    }
}

