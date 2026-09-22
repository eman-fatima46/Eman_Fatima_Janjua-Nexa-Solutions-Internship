using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController
    : ControllerBase
{
    private readonly IBookService
        _bookService;


    public BooksController(
        IBookService bookService)
    {
        _bookService =
            bookService;
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
    public async Task<IActionResult> GetById(
        int id)
    {
        var book =
            await _bookService
                .GetBookByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        BookRequestDto book)
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

        return Ok(
            new
            {
                message =
                    "Book created successfully."
            }
        );
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        BookRequestDto book)
    {
        bool updated =
            await _bookService
                .UpdateBookAsync(
                    id,
                    book
                );

        if (!updated)
        {
            return NotFound(
                "Book could not be updated."
            );
        }

        return Ok(
            new
            {
                message =
                    "Book updated successfully."
            }
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var existing =
            await _bookService
                .GetBookByIdAsync(id);

        if (existing == null)
        {
            return NotFound();
        }


        bool deleted =
            await _bookService
                .DeleteBookAsync(id);


        if (!deleted)
        {
            return BadRequest(
                "Book could not be deleted."
            );
        }


        return NoContent();
    }
}