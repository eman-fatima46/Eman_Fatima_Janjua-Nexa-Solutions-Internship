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
    public IActionResult GetAll()
    {
        var books =
            _bookService.GetAllBooks();

        return Ok(books);
    }


    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        Book? book =
            _bookService.GetBookById(id);

        if (book == null)
        {
            return NotFound(
                $"Book with ID {id} was not found."
            );
        }

        return Ok(book);
    }


    [HttpPost]
    public IActionResult Create(
        Book book)
    {
        Book? createdBook =
            _bookService.AddBook(book);

        if (createdBook == null)
        {
            return BadRequest(
                "Invalid book data or duplicate ID."
            );
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdBook.Id },
            createdBook
        );
    }


    [HttpPut("{id}")]
    public IActionResult Update(
        int id,
        Book book)
    {
        bool updated =
            _bookService.UpdateBook(id, book);

        if (!updated)
        {
            return BadRequest(
                "Book could not be updated."
            );
        }

        return Ok(book);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        bool deleted =
            _bookService.DeleteBook(id);

        if (!deleted)
        {
            return NotFound(
                $"Book with ID {id} was not found."
            );
        }

        return Ok(
            "Book deleted successfully."
        );
    }
}