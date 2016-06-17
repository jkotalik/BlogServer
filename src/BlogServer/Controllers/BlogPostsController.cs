using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogServer.Data;
using BlogServer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogServer.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: About
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // GET: Contact
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(int? id)
        {
            var list = await _context.BlogPost.ToListAsync();
            switch (id)
            {
                case 1:
                    list.Sort((a, b) => a.Date.CompareTo(b.Date));
                    break;
                case 2:
                    list.Sort((a, b) => b.VisitCount.CompareTo(a.VisitCount));
                    break;
                default:
                    // Any other options should result in normal ordering.
                    list.Sort((a, b) => b.Date.CompareTo(a.Date));
                    break;
            }

            // Check if user is allowed to edit post
            foreach (var item in list)
            {
                item.EditAndDeletePermissions = item.NameIdentifier == NameIdOfPost();
            }

            return View(list);
        }

        // GET: BlogPosts/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.EditAndDeletePermissions = blogPost.NameIdentifier == NameIdOfPost();
            blogPost.VisitCount++;
            await _context.SaveChangesAsync();
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Body,Title")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.NameIdentifier = NameIdOfPost();
                blogPost.LoginName = LoginNameOfPost();
                blogPost.Date = DateTime.Now;
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/id
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            if (blogPost.NameIdentifier != NameIdOfPost())
            {
                return Forbid();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/id
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Body,Title")] BlogPost blogPost)
        {
            if (id != blogPost.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var existingPost = await _context.BlogPost.SingleOrDefaultAsync(m => m.ID == id);
                    if (existingPost == null)
                    {
                        return NotFound();
                    }
                    if (existingPost.NameIdentifier != NameIdOfPost())
                    {
                        return Forbid();
                    }
                    // Modify the existing post rather than updating the whole row in the DB.
                    existingPost.Body = blogPost.Body;
                    existingPost.Title = blogPost.Title;
                    existingPost.EditedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.ID))
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
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.SingleOrDefaultAsync(m => m.ID == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            if (blogPost.NameIdentifier != NameIdOfPost())
            {
                return Forbid();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPost.SingleOrDefaultAsync(m => m.ID == id);
            _context.BlogPost.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPost.Any(e => e.ID == id);
        }

        private string NameIdOfPost()
        {
            if (this.User != null && this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                return this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return null;
        }

        private string LoginNameOfPost()
        {
            if (this.User != null && this.User.FindFirst(ClaimTypes.Name) != null)
            {
                return this.User.FindFirst(ClaimTypes.Name).Value;
            }
            return null;
        }
    }
}
