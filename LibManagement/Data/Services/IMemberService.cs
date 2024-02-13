using LibManagement.Models;
using LibManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LibManagement.Data.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAll();
        Task<Member> GetByID(string? id);
        Task<IEnumerable<Book>> GetCheckedOutBooks(string id);
        Task<double> GetFines(IEnumerable<Book> checkedOutBooks);
        MemberDetailVM SetForVM(Member member, IEnumerable<Book> books, double fines);
        Task<bool> Add(MemberCreate member, UserManager<Member> userManager,
            IUserStore<Member> userStore, IUserEmailStore<Member> emailStore);
        Task<bool> Delete(Member member);
        Task<bool> Update(Member newMember);
        bool MemberExists(string id);
    }
}
