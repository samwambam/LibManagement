using Azure.Core;
using LibManagement.Models;
using LibManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;

namespace LibManagement.Data.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibManagementContext _context;
        public MemberService(LibManagementContext context) { 
            _context = context;
        }

        public async Task<bool> Add(
            MemberCreate member,
            UserManager<Member> userManager,
            IUserStore<Member> userStore,
            IUserEmailStore<Member> emailStore
            )
        {
            var user = CreateUser();
            user.MemberID = user.Id;
            user.FullName = member.FullName;
            var password = "GERdwdwad^%4";

            await userStore.SetUserNameAsync(user, member.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, member.Email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Member");
            }

            return true;
        }

        private Member CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Member>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public async Task<bool> Delete(Member member)
        {
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Member>> GetAll()
        {
            return await _context.Member.ToListAsync();
        }

        public async Task<Member> GetByID(string? id)
        {
            return await _context.Member
                   .FirstOrDefaultAsync(m => m.MemberID == id);
        }

        public async Task<IEnumerable<Book>> GetCheckedOutBooks(string id)
        {
            Debug.WriteLine("Getting checked out");
            var data = await _context.Book.Where(b => b.MemberID == id).ToListAsync();
            return data;
        }

        public async Task<double> GetFines(IEnumerable<Book> checkedOutBooks)
        {
            var fines = await _context.Fine.ToListAsync();
            var maxFine = fines.First().MaxFine;
            var fineRate = fines.First().LateDayRate;

            var total = 0.0;
            foreach (var book in checkedOutBooks)
            {
                bool safe = (book.ReturnDate == null && DateTime.Now < book.DueDate) || (book.ReturnDate <= book.DueDate);
                if (!safe)
                {
                    int overdueDays = (book.ReturnDate.Value.Date - book.DueDate.Value.Date).Days;
                    var fineAmount = fineRate * overdueDays;
                    total += fineAmount;
                    if (total > maxFine)
                    {
                        return maxFine;
                    }
                }
            }
            return total;
        }

        public MemberDetailVM SetForVM(Member member, IEnumerable<Book> books, double fines)
        {
            MemberDetailVM memberDetail = new MemberDetailVM()
            {
                MemberID = member.MemberID,
                Books = books,
                FullName = member.FullName,
                Fine = fines,
                //Email = member.Email
            };
            return memberDetail;
        }

        public async Task<bool> Update(Member newMember)
        {
            _context.Member.Update(newMember);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

            }
            catch (OperationCanceledException)
            {

            }

            return true;
        }

        public bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
