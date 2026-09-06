using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(
        IBookRepository bookRepository)
    {
        _bookRepository =
            bookRepository;
    }


    public async Task<List<Book>>
        GetAllBooksAsync()
    {
        return await _bookRepository
            .GetAllAsync();
    }


    public async Task<Book?>
        GetBookByIdAsync(int id)
    {
        return await _bookRepository
            .GetByIdAsync(id);
    }


    public async Task<bool>
        AddBookAsync(Book book)
    {
        if (book.Id <= 0)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(
                book.Title))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(
                book.Author))
        {
            return false;
        }

        Book? existingBook =
            await _bookRepository
                .GetByIdAsync(book.Id);

        if (existingBook != null)
        {
            return false;
        }

        return await _bookRepository
            .AddAsync(book);
    }


    public async Task<bool>
        UpdateBookAsync(
            int id,
            Book book)
    {
        if (id != book.Id)
        {
            return false;
        }

        return await _bookRepository
            .UpdateAsync(book);
    }


    public async Task<bool>
        DeleteBookAsync(int id)
    {
        return await _bookRepository
            .DeleteAsync(id);
    }
}