using LibraryApi.DTOs;

namespace LibraryApi.Services;

public interface IBookService
{
    Task<List<BookResponseDto>>
        GetAllBooksAsync();

    Task<BookResponseDto?>
        GetBookByIdAsync(int id);

    Task<bool>
        AddBookAsync(
            BookRequestDto book);

    Task<bool>
        UpdateBookAsync(
            int id,
            BookRequestDto book);

    Task<bool>
        DeleteBookAsync(int id);
}