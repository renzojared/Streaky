using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    public readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public BookController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDTO>> Get(int id)
    {
        var books = await context.Book.FirstOrDefaultAsync(x => x.Id == id);
        return mapper.Map<BookDTO>(books);
    }

    [HttpPost]
    public async Task<ActionResult> Post(BookCreationDTO bookCreationDTO)
    {
        /*var existsAuthor = await context.Author.AnyAsync(x => x.Id == book.AuthorId);

        if (!existsAuthor)
            return BadRequest($"No existe el author de Id: {book.AuthorId}");*/
        var book = mapper.Map<Book>(bookCreationDTO);
        context.Add(book);

        await context.SaveChangesAsync();

        return Ok();
    }
}

