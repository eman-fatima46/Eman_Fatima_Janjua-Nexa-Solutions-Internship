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
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .ToListAsync();
    }


    public async Task<Book?> GetByIdAsync(
        int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(
                b => b.Id == id
            );
    }


    public async Task<bool> AddAsync(
        Book book,
        string authorName,
        string categoryName)
    {
        try
        {
            Author? author =
                await _context.Authors
                    .FirstOrDefaultAsync(
                        a =>
                            a.FullName
                            == authorName
                    );

            if (author == null)
            {
                author = new Author
                {
                    FullName = authorName
                };

                _context.Authors.Add(
                    author
                );
            }


            Category? category =
                await _context.Categories
                    .FirstOrDefaultAsync(
                        c =>
                            c.CategoryName
                            == categoryName
                    );

            if (category == null)
            {
                category =
                    new Category
                    {
                        CategoryName =
                            categoryName
                    };

                _context.Categories.Add(
                    category
                );
            }


            book.Author = author;

            book.BookCategories.Add(
                new BookCategory
                {
                    Book = book,
                    Category = category
                }
            );


            _context.Books.Add(
                book
            );

            await _context
                .SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                ex.InnerException?.Message
            );

            return false;
        }
    }


    public async Task<bool> UpdateAsync(
        Book book,
        string authorName,
        string categoryName)
    {
        try
        {
            Book? existingBook =
                await _context.Books
                    .Include(b => b.BookCategories)
                    .FirstOrDefaultAsync(
                        b => b.Id == book.Id
                    );

            if (existingBook == null)
            {
                return false;
            }


            Author? author =
                await _context.Authors
                    .FirstOrDefaultAsync(
                        a =>
                            a.FullName
                            == authorName
                    );

            if (author == null)
            {
                author = new Author
                {
                    FullName = authorName
                };

                _context.Authors.Add(
                    author
                );
            }


            Category? category =
                await _context.Categories
                    .FirstOrDefaultAsync(
                        c =>
                            c.CategoryName
                            == categoryName
                    );

            if (category == null)
            {
                category =
                    new Category
                    {
                        CategoryName =
                            categoryName
                    };

                _context.Categories.Add(
                    category
                );
            }


            existingBook.Title =
                book.Title;

            existingBook.Author =
                author;


            _context.BookCategories
                .RemoveRange(
                    existingBook
                        .BookCategories
                );


            existingBook
                .BookCategories
                .Add(
                    new BookCategory
                    {
                        Book =
                            existingBook,

                        Category =
                            category
                    }
                );


            await _context
                .SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                ex.InnerException?.Message
            );

            return false;
        }
    }


    public async Task<bool> DeleteAsync(
        int id)
    {
        try
        {
            Book? book =
                await _context.Books
                    .FirstOrDefaultAsync(
                        b => b.Id == id
                    );

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(
                book
            );

            await _context
                .SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                ex.InnerException?.Message
            );

            return false;
        }
    }
}