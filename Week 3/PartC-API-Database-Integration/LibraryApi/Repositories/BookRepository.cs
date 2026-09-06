using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(
        LibraryDbContext context)
    {
        _context = context;
    }


    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .ToListAsync();
    }


    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .FindAsync(id);
    }


    public async Task<bool> AddAsync(Book book)
    {
        try
        {
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("DATABASE ERROR:");
            Console.WriteLine(ex.Message);

            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            return false;
        }
    }


    public async Task<bool> UpdateAsync(Book book)
    {
        try
        {
            Book? existingBook =
                await _context.Books.FindAsync(
                    book.Id
                );

            if (existingBook == null)
            {
                return false;
            }

            existingBook.Title =
                book.Title;

            existingBook.Author =
                book.Author;

            existingBook.Category =
                book.Category;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }


    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            Book? book =
                await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}