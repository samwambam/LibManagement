using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibManagement.Models;
using LibManagement.Data.Services;
using System.Diagnostics;
using LibManagement.Models.ViewModels;
using System.Web.Razor;
using Microsoft.AspNetCore.Authorization;

namespace LibManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;

        public BooksController(IBookService bookService, IMemberService memberService)
        {
            _bookService = bookService;
            _memberService = memberService;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _bookService.GetAll());
        }

        public async Task<IActionResult> IndexDetail(int id)
        {
            return View(await _bookService.GetAllCopies(id));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //TO DO: existing titles and authors name check
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,ISBN,Genre")] Book book)
        { 
            if (ModelState.IsValid)
            {
                var success = await _bookService.Add(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,Genre,Available")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_bookService.BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookService.GetByID(id);
            if (book != null)
            {
                await _bookService.Delete(book);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BookCheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize] //TO DO: authorize
        public async Task<IActionResult> BookCheckOut(int id, Book newBook)
        {
            
            var memberID = newBook.MemberID;

            var book = await _bookService.GetByID(id);
            var member = await _memberService.GetByID(memberID);
            if (book == null || member == null)
            {
                return NotFound();
            }
            _bookService.UpdateForCheckOut(book, memberID);
            await _bookService.Update(book);
            
            return RedirectToAction(nameof(BookCheckOut), new { id});
        }
    }
}
