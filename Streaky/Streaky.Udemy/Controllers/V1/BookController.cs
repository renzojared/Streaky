using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class BookController : ControllerBase
{
    public readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public BookController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet("{id:int}", Name = "getBook")]
    public async Task<ActionResult<BookDTOWithAuthor>> Get(int id)
    {
        var books = await context.Book
            .Include(_ => _.AuthorBooks)
            .ThenInclude(ab => ab.Author)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (books == null)
            return NotFound();

        books.AuthorBooks = books.AuthorBooks.OrderBy(x => x.Order).ToList();

        return mapper.Map<BookDTOWithAuthor>(books);
    }

    [HttpPost(Name = "createBook")]
    public async Task<ActionResult> Post(BookCreationDTO bookCreationDTO)
    {
        if (bookCreationDTO.AuthorsId == null)
            return BadRequest("No se puede crear un libro sin autores.");

        var authors = await context.Author
            .Where(_ => bookCreationDTO.AuthorsId.Contains(_.Id)).Select(x => x.Id).ToListAsync();

        if (bookCreationDTO.AuthorsId.Count != authors.Count)
            return BadRequest("No existe alguno de los autores enviados.");

        var book = mapper.Map<Book>(bookCreationDTO);

        AssignOrderAuthors(book);

        context.Add(book);

        await context.SaveChangesAsync();

        var bookDTO = mapper.Map<BookDTO>(book);

        return CreatedAtRoute("getBook", new { id = book.Id }, bookDTO);
    }

    [HttpPut("{id:int}", Name = "updateBook")]
    public async Task<ActionResult> Put(int id, BookCreationDTO bookCreationDTO)
    {
        var bookDb = await context.Book
            .Include(_ => _.AuthorBooks)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (bookDb == null)
            return NotFound();

        bookDb = mapper.Map(bookCreationDTO, bookDb); //persistir cambios en una sola variable declarada.

        AssignOrderAuthors(bookDb);

        await context.SaveChangesAsync();

        return NoContent();
    }

    private void AssignOrderAuthors(Book book)
    {
        if (book.AuthorBooks != null)
        {
            for (int i = 0; i < book.AuthorBooks.Count; i++)
            {
                book.AuthorBooks[i].Order = i;
            }
        }
    }

    [HttpPatch("{id:int}", Name = "patchBook")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> jsonPatchDocument)
    {
        if (jsonPatchDocument == null)
            return BadRequest();

        var bookDB = await context.Book.FirstOrDefaultAsync(x => x.Id == id);

        if (bookDB == null)
            return NotFound();

        var bookDTO = mapper.Map<BookPatchDTO>(bookDB);

        jsonPatchDocument.ApplyTo(bookDTO, ModelState);

        var isValid = TryValidateModel(bookDTO);

        if (!isValid)
            return BadRequest(ModelState);

        mapper.Map(bookDTO, bookDB);

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "deleteBook")]
    public async Task<ActionResult> Delete(int id)
    {
        var exists = await context.Book.AnyAsync(x => x.Id == id);

        if (!exists)
            return NotFound();

        context.Remove(new Book() { Id = id });
        await context.SaveChangesAsync();

        return NoContent();
    }
}

