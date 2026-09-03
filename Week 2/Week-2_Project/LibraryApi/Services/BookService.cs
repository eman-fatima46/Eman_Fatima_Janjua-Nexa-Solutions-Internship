using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(
        IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAll();
    }

    public Book? GetBookById(int id)
    {
        return _bookRepository.GetById(id);
    }

    public Book? AddBook(Book book)
    {
        if (book.Id <= 0 ||
            string.IsNullOrWhiteSpace(book.Title) ||
            string.IsNullOrWhiteSpace(book.Author))
        {
            return null;
        }

        Book? existingBook =
            _bookRepository.GetById(book.Id);

        if (existingBook != null)
        {
            return null;
        }

        _bookRepository.Add(book);

        return book;
    }

    public bool UpdateBook(
        int id,
        Book book)
    {
        if (id != book.Id)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(book.Title) ||
            string.IsNullOrWhiteSpace(book.Author))
        {
            return false;
        }

        return _bookRepository.Update(book);
    }

    public bool DeleteBook(int id)
    {
        return _bookRepository.Delete(id);
    }
}