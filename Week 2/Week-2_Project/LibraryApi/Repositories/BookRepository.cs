using LibraryApi.Models;

namespace LibraryApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly List<Book> _books =
    [
        new Book
        {
            Id = 1,
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Category = "Programming"
        },

        new Book
        {
            Id = 2,
            Title = "The Pragmatic Programmer",
            Author = "Andrew Hunt",
            Category = "Programming"
        },

        new Book
        {
            Id = 3,
            Title = "Artificial Intelligence Basics",
            Author = "Tom Taulli",
            Category = "Artificial Intelligence"
        }
    ];

    public List<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(
            book => book.Id == id
        );
    }

    public void Add(Book book)
    {
        _books.Add(book);
    }

    public bool Update(Book updatedBook)
    {
        Book? existingBook = GetById(updatedBook.Id);

        if (existingBook == null)
        {
            return false;
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.Category = updatedBook.Category;

        return true;
    }

    public bool Delete(int id)
    {
        Book? book = GetById(id);

        if (book == null)
        {
            return false;
        }

        _books.Remove(book);

        return true;
    }
}