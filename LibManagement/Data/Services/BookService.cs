using LibManagement.Models;
using LibManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibManagement.Data.Services
{
    public class BookService : IBookService
    {
        private readonly LibManagementContext _context;

        public BookService(LibManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Book book)
        {
            book.Available = true;
            _context.Book.Add(book); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Book book)
        {
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookIndexVM>> GetAll()
        {
            var data = await _context.Book.ToListAsync();
            Dictionary<string,BookIndexVM> books = new Dictionary<string, BookIndexVM>();
            foreach (var book in data)
            {
                string key = (book.Title + book.Author).ToLower();
                bool exists = books.ContainsKey(key);
                if (exists)
                {
                    BookIndexVM b = books[key];
                    int newTotal = b.Total + 1;
                    int newQuantity = book.Available ? b.QuantityAvailable + 1 : b.QuantityAvailable;
                    BookIndexVM copy = new BookIndexVM
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        ISBN = b.ISBN,
                        Genre = b.Genre,
                        Total = newTotal,
                        QuantityAvailable = newQuantity
                    };
                    books[key] = copy;
                    
                }
                else
                {
                    BookIndexVM newBook = new BookIndexVM
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        ISBN = book.ISBN,
                        Genre = book.Genre,
                        Total = 1,
                        QuantityAvailable = book.Available ? 1 : 0
                };
                    books.Add(key, newBook);
                }

            }
            List<BookIndexVM> bookData = books.Values.ToList<BookIndexVM>();
            //return data;
            return bookData;
        }

        public async Task<Book> GetByID(int? id)
        {
            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetAllCopies(int id)
        {
            List<Book> books = new List<Book>();
            var book = await _context.Book
                .FirstOrDefaultAsync(b => b.Id == id);
            var copies = await _context.Book.Where(b => b.Title == book.Title && b.Author == book.Author).ToListAsync();
            books = copies;
            return books;
        }

        public async Task<bool> Update(Book newBook)
        {
            _context.Book.Update(newBook);
            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateException ex)
            {

            }catch(OperationCanceledException)
            {

            }
            
            return true;
        }

        public void UpdateForCheckOut(Book book, string memberID)
        {
            book.Available = false;
            book.MemberID = memberID;
            book.DueDate = DateTime.Now.AddDays(30);
        }

        public void UpdateForReturn(Book book)
        {
            book.Available = true;
            book.MemberID = null;
            book.ReturnDate = DateTime.Now;
        }

        public bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
