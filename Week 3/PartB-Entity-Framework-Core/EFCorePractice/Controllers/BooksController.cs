using EFCorePractice.Data;
using EFCorePractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePractice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public BooksController(
        LibraryDbContext context)
    {
        _context = context;
    }


    [HttpPost("seed")]
    public async Task<IActionResult> AddSampleBooks()
    {
        if (await _context.Books.AnyAsync())
        {
            return BadRequest(
                "Books already exist."
            );
        }

        _context.Books.Add(
            new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin"
            }
        );

        _context.Books.Add(
            new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt"
            }
        );

        _context.Books.Add(
            new Book
            {
                Title = "AI Basics",
                Author = "Tom Taulli"
            }
        );

        await _context.SaveChangesAsync();

        return Ok(
            "Sample books inserted."
        );
    }


    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        List<Book> books =
            await _context.Books.ToListAsync();

        return Ok(books);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(
        int id,
        Book updatedBook)
    {
        Book? book =
            await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;

        await _context.SaveChangesAsync();

        return Ok(book);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(
        int id)
    {
        Book? book =
            await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);

        await _context.SaveChangesAsync();

        return Ok(
            "Book deleted successfully."
        );
    }


    [HttpGet("author/{authorName}")]
    public async Task<IActionResult> GetBooksByAuthor(
        string authorName)
    {
        List<Book> books =
            await _context.Books
                .Where(book =>
                    book.Author == authorName)
                .ToListAsync();

        return Ok(books);
    }
}