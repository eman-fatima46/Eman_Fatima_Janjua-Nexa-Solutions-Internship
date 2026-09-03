using LibraryApi.Models;

namespace LibraryApi.Services;

public interface IBookService
{
    List<Book> GetAllBooks();

    Book? GetBookById(int id);

    Book? AddBook(Book book);

    bool UpdateBook(int id, Book book);

    bool DeleteBook(int id);
}