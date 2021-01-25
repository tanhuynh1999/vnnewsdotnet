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
using System.IO;

namespace vnnews.Areas.Admin.Controllers
{
    public class NewsAdminController : Controller
    {
        private vnnewsEntities db = new vnnewsEntities();

        // GET: Admin/NewsAdmin
        public async Task<ActionResult> Index()
        {
            var news = db.News.Include(n => n.User).Where(t => t.news_bin == false);
            return View(await news.ToListAsync());
        }

        // GET: Admin/NewsAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/NewsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email");
            return View();
        }

        // POST: Admin/NewsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "news_id,news_name,news_content,news_datecreate,news_view,news_like,news_img,news_video,news_active,news_bin,user_id")] News news, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                // default information
                news.news_datecreate = DateTime.Now;
                news.news_active = true;
                news.news_bin = false;
                news.user_id = int.Parse(Response.Cookies["user_cookie"].Value.ToString());

                // add image and replace random name
                string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                file_img.SaveAs(pathimg);
                news.news_img = fileName;

                db.News.Add(news);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", news.user_id);
            return View(news);
        }

        // GET: Admin/NewsAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", news.user_id);
            return View(news);
        }

        // POST: Admin/NewsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "news_id,news_name,news_content,news_datecreate,news_view,news_like,news_img,news_video,news_active,news_bin,user_id")] News news, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                if (file_img != null)
                {
                    // remove old image
                    if (news.news_img != null)
                    {
                        string fullPath = Request.MapPath("~/Images/" + news.news_img);
                        System.IO.File.Delete(fullPath);
                    }
                    // add image and replace random name
                    string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                    var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file_img.SaveAs(pathimg);
                    news.news_img = fileName;
                }
                db.Entry(news).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", news.user_id);
            return View(news);
        }

        // GET: Admin/NewsAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/NewsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            News news = await db.News.FindAsync(id);
            db.News.Remove(news);
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
