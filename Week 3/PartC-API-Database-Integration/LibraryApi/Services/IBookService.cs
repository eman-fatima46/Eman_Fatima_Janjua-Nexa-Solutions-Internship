using LibraryApi.Models;

namespace LibraryApi.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();

    Task<Book?> GetBookByIdAsync(int id);

    Task<bool> AddBookAsync(Book book);

    Task<bool> UpdateBookAsync(
        int id,
        Book book
    );

    Task<bool> DeleteBookAsync(int id);
}