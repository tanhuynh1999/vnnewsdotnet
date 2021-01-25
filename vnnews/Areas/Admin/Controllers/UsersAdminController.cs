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
    public class UsersAdminController : Controller
    {
        private vnnewsEntities db = new vnnewsEntities();

        // GET: Admin/UsersAdmin
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.Role).Include(u => u.Sex).Where(t => t.user_bin == false);
            return View(await users.ToListAsync());
        }

        // GET: Admin/UsersAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/UsersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name");
            ViewBag.sex_id = new SelectList(db.Sexs, "sex_id", "sex_name");
            return View();
        }

        // POST: Admin/UsersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "user_id,user_email,user_pass,user_datecreate,user_datelogin,role_id,user_name,user_img,sex_id,user_phone,user_active,user_bin")] User user, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                // default information
                user.user_datecreate = DateTime.Now;
                user.user_active = true;
                user.user_bin = false;

                // add image and replace random name
                string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                file_img.SaveAs(pathimg);
                user.user_img = fileName;

                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            ViewBag.sex_id = new SelectList(db.Sexs, "sex_id", "sex_name", user.sex_id);
            return View(user);
        }

        // GET: Admin/UsersAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            ViewBag.sex_id = new SelectList(db.Sexs, "sex_id", "sex_name", user.sex_id);
            return View(user);
        }

        // POST: Admin/UsersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "user_id,user_email,user_pass,user_datecreate,user_datelogin,role_id,user_name,user_img,sex_id,user_phone,user_active,user_bin")] User user, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                if (file_img != null)
                {
                    // remove old image
                    if (user.user_img != null)
                    {
                        string fullPath = Request.MapPath("~/Images/" + user.user_img);
                        System.IO.File.Delete(fullPath);
                    }
                    // add image and replace random name
                    string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                    var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file_img.SaveAs(pathimg);
                    user.user_img = fileName;
                }

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.Roles, "role_id", "role_name", user.role_id);
            ViewBag.sex_id = new SelectList(db.Sexs, "sex_id", "sex_name", user.sex_id);
            return View(user);
        }

        // GET: Admin/UsersAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
