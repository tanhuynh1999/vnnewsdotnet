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
    public class CategoriesAdminController : Controller
    {
        private vnnewsEntities db = new vnnewsEntities();

        // GET: Admin/CategoriesAdmin
        public async Task<ActionResult> Index()
        {
            return View(await db.Categorys.Where(t => t.category_bin == false).ToListAsync());
        }

        // GET: Admin/CategoriesAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categorys.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoriesAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "category_id,category_name,category_img,category_datecreate,category_view,category_active,category_bin")] Category category, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                // default information
                category.category_datecreate = DateTime.Now;
                category.category_active = true;
                category.category_bin = false;

                // add image and replace random name
                string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                file_img.SaveAs(pathimg);
                category.category_img = fileName;

                db.Categorys.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categorys.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/CategoriesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "category_id,category_name,category_img,category_datecreate,category_view,category_active,category_bin")] Category category, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                if (file_img != null)
                {
                    // remove old image
                    if (category.category_img != null)
                    {
                        string fullPath = Request.MapPath("~/Images/" + category.category_img);
                        System.IO.File.Delete(fullPath);
                    }
                    // add image and replace random name
                    string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                    var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file_img.SaveAs(pathimg);
                    category.category_img = fileName;
                }
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/CategoriesAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categorys.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/CategoriesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categorys.FindAsync(id);
            db.Categorys.Remove(category);
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
