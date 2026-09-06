using LibraryApi.Models;

namespace LibraryApi.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<bool> AddAsync(
        Book book,
        string authorName,
        string categoryName
    );

    Task<bool> UpdateAsync(
        Book book,
        string authorName,
        string categoryName
    );

    Task<bool> DeleteAsync(int id);
}