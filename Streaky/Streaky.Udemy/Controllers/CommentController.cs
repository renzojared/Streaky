using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("api/book/{bookId:int}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public CommentController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
    {
        var comments = await context.Comment.Where(_ => _.BookId == bookId).ToListAsync();

        var existBook = await context.Book.AnyAsync(_ => _.Id == bookId);

        if (!existBook)
            return NotFound();

        return mapper.Map<List<CommentDTO>>(comments);
    }

    [HttpGet("{id:int}", Name = "getComment")]
    public async Task<ActionResult<CommentDTO>> GetById(int id)
    {
        var comment = await context.Comment.FirstOrDefaultAsync(_ => _.Id == id);

        if (comment == null)
            return NotFound();

        return mapper.Map<CommentDTO>(comment);
    }

    [HttpPost]
    public async Task<ActionResult> Post(int bookId, CommentCreationDTO commentCreationDTO)
    {
        var existBook = await context.Book.AnyAsync(_ => _.Id == bookId);

        if (!existBook)
            return NotFound();

        var comment = mapper.Map<Comment>(commentCreationDTO);
        comment.BookId = bookId;
        context.Add(comment);
        await context.SaveChangesAsync();

        var commentDTO = mapper.Map<CommentDTO>(comment);

        return CreatedAtRoute("getComment", new { id = comment.Id, bookId = bookId }, commentDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int bookId, int id, CommentCreationDTO commentCreationDTO)
    {
        var existBook = await context.Book.AnyAsync(_ => _.Id == bookId);

        if (!existBook)
            return NotFound();

        var existsComment = await context.Comment.AnyAsync(_ => _.Id == id);

        if (!existsComment)
            return NotFound();

        var comment = mapper.Map<Comment>(commentCreationDTO);
        comment.Id = id;
        comment.BookId = bookId;
        context.Update(comment);
        await context.SaveChangesAsync();

        return NoContent();
    }

}

