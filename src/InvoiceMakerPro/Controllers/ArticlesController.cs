using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using InvoiceMakerPro.Models;
using System;
using Microsoft.AspNet.Authorization;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace InvoiceMakerPro.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Articles
        public IActionResult Index()
        {
            return View(_context.Article.ToList());
        }

        // GET: Articles/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Article article = _context.Article.Single(m => m.ArticleId == id);
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.ArticleId = Guid.NewGuid();
                _context.Article.Add(article);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Article article = _context.Article.Single(m => m.ArticleId == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Update(article);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Article article = _context.Article.Single(m => m.ArticleId == id);
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            Article article = _context.Article.Single(m => m.ArticleId == id);
            _context.Article.Remove(article);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetArticles([DataSourceRequest]DataSourceRequest request)
        {
            return Json(_context.Article.ToDataSourceResult(request));
        }
    }
}
