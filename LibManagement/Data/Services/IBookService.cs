using LibManagement.Models;
using LibManagement.Models.ViewModels;

namespace LibManagement.Data.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookIndexVM>> GetAll();
        Task<IEnumerable<Book>> GetAllCopies(int id);
        Task<Book> GetByID(int? id);
        Task<bool> Add(Book book);
        Task<bool> Update(Book newBook);
        Task<bool> Delete(Book book);
        void UpdateForCheckOut(Book book, string memberID);
        void UpdateForReturn(Book book);
        bool BookExists(int id);
    }
}
