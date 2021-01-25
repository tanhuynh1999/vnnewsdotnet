using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using vnnews.Models;

namespace vnnews.Areas.Admin.Controllers
{
    public class AdsAdminController : Controller
    {
        private vnnewsEntities db = new vnnewsEntities();

        // GET: Admin/AdsAdmin
        public ActionResult Index()
        {
            var ads = db.Ads.Include(a => a.User).Where(t => t.ad_bin == false);
            return View(ads.ToList());
        }

        // GET: Admin/AdsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // GET: Admin/AdsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email");
            return View();
        }

        // POST: Admin/AdsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ad_id,ad_img,ad_datecreate,ad_dateupdate,ad_note,user_id,ad_active,ad_bin")] Ad ad, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                // default information
                ad.ad_datecreate = DateTime.Now;
                ad.ad_dateupdate = DateTime.Now;
                ad.ad_active = true;
                ad.ad_bin = false;
                ad.user_id = int.Parse(Response.Cookies["user_cookie"].Value.ToString());

                // add image and replace random name
                string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                file_img.SaveAs(pathimg);
                ad.ad_img = fileName;

                // add to database
                db.Ads.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", ad.user_id);
            return View(ad);
        }

        // GET: Admin/AdsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", ad.user_id);
            return View(ad);
        }

        // POST: Admin/AdsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ad_id,ad_img,ad_datecreate,ad_dateupdate,ad_note,user_id,ad_active,ad_bin")] Ad ad, HttpPostedFileBase file_img)
        {
            if (ModelState.IsValid)
            {
                ad.ad_dateupdate = DateTime.Now;
                if(file_img != null)
                {
                    // remove old image
                    if(ad.ad_img != null)
                    {
                        string fullPath = Request.MapPath("~/Images/" + ad.ad_img);
                        System.IO.File.Delete(fullPath);
                    }
                    // add image and replace random name
                    string fileName = Guid.NewGuid() + Path.GetExtension(file_img.FileName);
                    var pathimg = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file_img.SaveAs(pathimg);
                    ad.ad_img = fileName;
                }
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.Users, "user_id", "user_email", ad.user_id);
            return View(ad);
        }

        // GET: Admin/AdsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Admin/AdsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ad ad = db.Ads.Find(id);
            db.Ads.Remove(ad);
            db.SaveChanges();
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
