using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SamsBookReviewLibary.Data;
using SamsBookReviewLibary.Models;

namespace SamsBookReviewLibary.Controllers
{
    public class BookTitleGenresController : Controller
    {
        private readonly AuthorContext _context;

        public BookTitleGenresController(AuthorContext context)
        {
            _context = context;    
        }


        public async Task<IActionResult> Index()
        {
            var authorContext = _context.BookTitleGenres.Include(b => b.BookTitle).Include(b => b.Genre);
            return View(await authorContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTitleGenres = await _context.BookTitleGenres
                .Include(b => b.BookTitle)
                .Include(b => b.Genre)
                .SingleOrDefaultAsync(m => m.BookTitleGenresID == id);
            if (bookTitleGenres == null)
            {
                return NotFound();
            }

            return View(bookTitleGenres);
        }

        public IActionResult Create()
        {
            ViewData["BookTitleID"] = new SelectList(_context.BookTitles.OrderBy(b =>b.Title), "BookTitleID", "Title");
            ViewData["GenreID"] = new SelectList(_context.Genres.OrderBy(g =>g.GenreTypes), "GenreID", "GenreTypes");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookTitleGenresID,GenreID,BookTitleID")] BookTitleGenres bookTitleGenres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookTitleGenres);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["BookTitleID"] = new SelectList(_context.BookTitles, "BookTitleID", "BookTitleID", bookTitleGenres.BookTitleID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", bookTitleGenres.GenreID);
            return View(bookTitleGenres);
        }

        // GET: BookTitleGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTitleGenres = await _context.BookTitleGenres.SingleOrDefaultAsync(m => m.BookTitleGenresID == id);
            if (bookTitleGenres == null)
            {
                return NotFound();
            }
            ViewData["BookTitleID"] = new SelectList(_context.BookTitles.OrderBy(b =>b.Title), "BookTitleID", "Title", bookTitleGenres.BookTitleID);
            ViewData["GenreID"] = new SelectList(_context.Genres.OrderBy(g =>g.GenreTypes), "GenreID", "GenreTypes", bookTitleGenres.GenreID);
            return View(bookTitleGenres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookTitleGenresID,GenreID,BookTitleID")] BookTitleGenres bookTitleGenres)
        {
            if (id != bookTitleGenres.BookTitleGenresID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookTitleGenres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookTitleGenresExists(bookTitleGenres.BookTitleGenresID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["BookTitleID"] = new SelectList(_context.BookTitles, "BookTitleID", "BookTitleID", bookTitleGenres.BookTitleID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", bookTitleGenres.GenreID);
            return View(bookTitleGenres);
        }

        // GET: BookTitleGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookTitleGenres = await _context.BookTitleGenres
                .Include(b => b.BookTitle)
                .Include(b => b.Genre)
                .SingleOrDefaultAsync(m => m.BookTitleGenresID == id);
            if (bookTitleGenres == null)
            {
                return NotFound();
            }

            return View(bookTitleGenres);
        }

        // POST: BookTitleGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookTitleGenres = await _context.BookTitleGenres.SingleOrDefaultAsync(m => m.BookTitleGenresID == id);
            _context.BookTitleGenres.Remove(bookTitleGenres);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BookTitleGenresExists(int id)
        {
            return _context.BookTitleGenres.Any(e => e.BookTitleGenresID == id);
        }
    }
}
