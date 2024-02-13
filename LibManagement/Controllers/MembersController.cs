using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibManagement.Data;
using LibManagement.Models;
using LibManagement.Data.Services;
using LibManagement.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace LibManagement.Controllers
{
    //TO DO: authorize
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBookService _bookService;

        private readonly UserManager<Member> _userManager;
        private readonly IUserStore<Member> _userStore;
        private readonly IUserEmailStore<Member> _emailStore;

        public MembersController(
            IMemberService service, 
            IBookService bookService,
            UserManager<Member> userManager,
            IUserStore<Member> userStore
            )
        {
            _memberService = service;
            _bookService = bookService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }

        private IUserEmailStore<Member> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Member>)_userStore;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _memberService.GetAll());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member member = await _memberService.GetByID(id);
            var checkedOutBooks = await _memberService.GetCheckedOutBooks(id);
            var fines = await _memberService.GetFines(checkedOutBooks);
            if (Request.Query["booksError"] == "true")
            {
                ModelState.AddModelError(nameof(MemberDetailVM.Books), "Member still has checked out books");
            }
            MemberDetailVM memberDetail = _memberService.SetForVM(member, checkedOutBooks, fines);
            if (member == null)
            {
                return NotFound();
            }

            return View(memberDetail);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email")] MemberCreate member)
        {
            if (ModelState.IsValid)
            {
                await _memberService.Add(member, _userManager, _userStore, _emailStore);
                return RedirectToAction(nameof(Index));
            }
            /*
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            */
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberService.GetByID(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,FullName,Email")] Member member)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _memberService.Update(member);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_memberService.MemberExists(member.MemberID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberService.GetByID(id);
            var checkedOutBooks = await _memberService.GetCheckedOutBooks(id);
            if (member == null)
            {
                return NotFound();
            }else if (checkedOutBooks.Count() > 0)
            {
                return RedirectToAction(nameof(Details), new {id, booksError = "true" });
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _memberService.GetByID(id);
            if (member != null)
            {
                await _memberService.Delete(member);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(string id, int bookId)
        {
            var book = await _bookService.GetByID(bookId);
            if (book != null)
            {
                _bookService.UpdateForReturn(book);
                await _bookService.Update(book);
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}