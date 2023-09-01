using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;
using Streaky.Udemy.Utilities;

namespace Streaky.Udemy.Controllers.V1;

[ApiController]
[Route("api/v1/book/{bookId:int}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public CommentController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        this.context = context;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpGet(Name = "getCommentsBook")]
    public async Task<ActionResult<List<CommentDTO>>> Get(int bookId, [FromQuery] PaginationDTO paginationDTO)
    {

        var existBook = await context.Book.AnyAsync(_ => _.Id == bookId);

        if (!existBook)
            return NotFound();

        var queryable = context.Comment.Where(_ => _.BookId == bookId).AsQueryable();
        await HttpContext.InsertParameteresPaginationInHeader(queryable);

        var comments = await queryable.OrderBy(c => c.Id).ToPage(paginationDTO).ToListAsync();

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

    [HttpPost(Name = "createComment")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Post(int bookId, CommentCreationDTO commentCreationDTO)
    {
        var emailClaim = HttpContext.User.Claims.Where(c => c.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var user = await userManager.FindByEmailAsync(email);
        var userId = user.Id; 

        var existBook = await context.Book.AnyAsync(_ => _.Id == bookId);

        if (!existBook)
            return NotFound();

        var comment = mapper.Map<Comment>(commentCreationDTO);
        comment.BookId = bookId;
        comment.UserId = userId;
        context.Add(comment);
        await context.SaveChangesAsync();

        var commentDTO = mapper.Map<CommentDTO>(comment);

        return CreatedAtRoute("getComment", new { id = comment.Id, bookId = bookId }, commentDTO);
    }

    [HttpPut("{id:int}", Name = "updateComment")]
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

