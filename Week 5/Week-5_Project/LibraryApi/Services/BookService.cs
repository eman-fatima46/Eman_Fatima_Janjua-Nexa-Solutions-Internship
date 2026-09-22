using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository
        _repository;


    public BookService(
        IBookRepository repository)
    {
        _repository = repository;
    }


    public async Task<List<BookResponseDto>>
        GetAllBooksAsync()
    {
        List<Book> books =
            await _repository
                .GetAllAsync();

        return books
            .Select(MapBook)
            .ToList();
    }


    public async Task<BookResponseDto?>
        GetBookByIdAsync(
            int id)
    {
        Book? book =
            await _repository
                .GetByIdAsync(id);

        if (book == null)
        {
            return null;
        }

        return MapBook(book);
    }


    public async Task<bool>
        AddBookAsync(
            BookRequestDto request)
    {
        if (
            string.IsNullOrWhiteSpace(
                request.Title
            ) ||
            string.IsNullOrWhiteSpace(
                request.Author
            ) ||
            string.IsNullOrWhiteSpace(
                request.Category
            )
        )
        {
            return false;
        }

        Book book =
            new Book
            {
                Title =
                    request.Title
            };

        return await _repository
            .AddAsync(
                book,
                request.Author,
                request.Category
            );
    }


    public async Task<bool>
        UpdateBookAsync(
            int id,
            BookRequestDto request)
    {
        if (
            request.Id != 0 &&
            request.Id != id
        )
        {
            return false;
        }

        Book book =
            new Book
            {
                Id = id,
                Title =
                    request.Title
            };

        return await _repository
            .UpdateAsync(
                book,
                request.Author,
                request.Category
            );
    }


    public async Task<bool>
        DeleteBookAsync(
            int id)
    {
        return await _repository
            .DeleteAsync(id);
    }


    private static BookResponseDto
        MapBook(
            Book book)
    {
        return new BookResponseDto
        {
            Id = book.Id,

            Title = book.Title,

            Author =
                book.Author?.FullName
                ?? string.Empty,

            Category =
                string.Join(
                    ", ",
                    book.BookCategories
                        .Where(
                            bc =>
                                bc.Category
                                != null
                        )
                        .Select(
                            bc =>
                                bc.Category!
                                    .CategoryName
                        )
                )
        };
    }
}