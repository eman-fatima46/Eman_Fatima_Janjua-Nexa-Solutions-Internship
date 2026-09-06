using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(
        IBookService bookService)
    {
        _bookService = bookService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books =
            await _bookService
                .GetAllBooksAsync();

        return Ok(books);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetById(int id)
    {
        Book? book =
            await _bookService
                .GetBookByIdAsync(id);

        if (book == null)
        {
            return NotFound(
                $"Book with ID {id} was not found."
            );
        }

        return Ok(book);
    }


    [HttpPost]
    public async Task<IActionResult>
        Create(Book book)
    {
        try
        {
            bool created =
                await _bookService
                    .AddBookAsync(book);

            if (!created)
            {
                return BadRequest(
                    "Book could not be created."
                );
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                "A database error occurred while creating the book."
            );
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult>
        Update(
            int id,
            Book book)
    {
        try
        {
            bool updated =
                await _bookService
                    .UpdateBookAsync(
                        id,
                        book
                    );

            if (!updated)
            {
                return BadRequest(
                    "Book could not be updated."
                );
            }

            return Ok(book);
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                "A database error occurred while updating the book."
            );
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult>
        Delete(int id)
    {
        Book? existingBook =
            await _bookService
                .GetBookByIdAsync(id);

        if (existingBook == null)
        {
            return NotFound(
                $"Book with ID {id} was not found."
            );
        }

        try
        {
            bool deleted =
                await _bookService
                    .DeleteBookAsync(id);

            if (!deleted)
            {
                return StatusCode(
                    500,
                    "Book could not be deleted."
                );
            }

            return Ok(
                "Book deleted successfully."
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                "A database error occurred while deleting the book."
            );
        }
    }
}