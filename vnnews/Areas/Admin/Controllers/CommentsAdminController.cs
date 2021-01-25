using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using vnnews.Models;

namespace vnnews.Areas.Admin.Controllers
{
    public class CommentsAdminController : Controller
    {
        private vnnewsEntities db = new vnnewsEntities();

        // GET: Admin/CommentsAdmin
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.News).Include(c => c.User);
            return View(await comments.ToListAsync());
        }

        // GET: Admin/CommentsAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.news_id = new SelectList(db.News, "news_id", "news_name");
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email");
            return View();
        }

        // POST: Admin/CommentsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "comment_id,comment_content,comment_datecreate,comment_dateupdate,comment_active,news_id,user_id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.news_id = new SelectList(db.News, "news_id", "news_name", comment.news_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", comment.user_id);
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.news_id = new SelectList(db.News, "news_id", "news_name", comment.news_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", comment.user_id);
            return View(comment);
        }

        // POST: Admin/CommentsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "comment_id,comment_content,comment_datecreate,comment_dateupdate,comment_active,news_id,user_id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.news_id = new SelectList(db.News, "news_id", "news_name", comment.news_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", comment.user_id);
            return View(comment);
        }

        // GET: Admin/CommentsAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Admin/CommentsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
