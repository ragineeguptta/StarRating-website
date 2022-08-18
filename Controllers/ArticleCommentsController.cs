using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarRating.Data;
using StarRating.Models;
using StarRating.ViewModel;

namespace StarRating.Controllers
{
    public class ArticleCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ArticleComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ArticlesComments.Include(a => a.Articles);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ArticleCommentViewModel vm)
        {
            var comment = vm.Comment;
            var articleId = vm.ArticlesId;
            var rating = vm.Rating;

            ArticleComment artComment = new ArticleComment()
            {
                ArticlesId = articleId,
                Comments = comment,
                Rating = rating,
                PublishedDate = DateTime.Now
            };

            _context.ArticlesComments.Add(artComment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Articles", new { id = articleId });
        }

        // GET: ArticleComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments
                .Include(a => a.Articles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleComment == null)
            {
                return NotFound();
            }

            return View(articleComment);
        }

        // GET: ArticleComments/Create
        public IActionResult Create()
        {
            ViewData["ArticlesId"] = new SelectList(_context.Articles, "Id", "Id");
            return View();
        }

        // POST: ArticleComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments,PublishedDate,ArticlesId,Rating")] ArticleComment articleComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticlesId"] = new SelectList(_context.Articles, "Id", "Id", articleComment.ArticlesId);
            return View(articleComment);
        }

        // GET: ArticleComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments.FindAsync(id);
            if (articleComment == null)
            {
                return NotFound();
            }
            ViewData["ArticlesId"] = new SelectList(_context.Articles, "Id", "Id", articleComment.ArticlesId);
            return View(articleComment);
        }

        // POST: ArticleComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments,PublishedDate,ArticlesId,Rating")] ArticleComment articleComment)
        {
            if (id != articleComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleCommentExists(articleComment.Id))
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
            ViewData["ArticlesId"] = new SelectList(_context.Articles, "Id", "Id", articleComment.ArticlesId);
            return View(articleComment);
        }

        // GET: ArticleComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleComment = await _context.ArticlesComments
                .Include(a => a.Articles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleComment == null)
            {
                return NotFound();
            }

            return View(articleComment);
        }

        // POST: ArticleComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleComment = await _context.ArticlesComments.FindAsync(id);
            _context.ArticlesComments.Remove(articleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleCommentExists(int id)
        {
            return _context.ArticlesComments.Any(e => e.Id == id);
        }
    }
}
