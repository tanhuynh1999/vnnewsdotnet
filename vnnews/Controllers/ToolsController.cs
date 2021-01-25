using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnnews.Models;

namespace vnnews.Controllers
{
    public class ToolsController : Controller
    {
        vnnewsEntities db = new vnnewsEntities();
        // GET: Tools
        //Danh sach yêu thích details
        public JsonResult JsonFavouriteD(int ?id)
        {
            HttpCookie cookie = Request.Cookies["user_cookie"];
            User user = db.Users.Find(int.Parse(cookie.Value.ToString()));

            List<Favourite> Favourite = db.Favourites.Where(n=>n.news_id == id && n.user_id == user.user_id).ToList();
            List<JsonFavourite> list = Favourite.Select(n => new JsonFavourite
            {
                datecreate = n.favourite_datecreate.ToString(),
                id = n.favourite_id,
                idnews = (int)n.news_id,
                idus = (int)n.user_id

            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Favourite(int? idus, int? idnews)
        {
            HttpCookie cookie = Request.Cookies["user_cookie"];
            User user = db.Users.Find(int.Parse(cookie.Value.ToString()));

            Favourite fa = new Favourite()
            {
                favourite_datecreate = DateTime.Now,
                news_id = idnews,
                user_id = idus
            };
            db.Favourites.Add(fa);
            db.SaveChanges();

            List<Favourite> Favourite = db.Favourites.Where(n => n.news_id == idnews && n.user_id == user.user_id).ToList();
            List<JsonFavourite> list = Favourite.Select(n => new JsonFavourite
            {
                datecreate = n.favourite_datecreate.ToString(),
                id = n.favourite_id,
                idnews = (int)n.news_id,
                idus = (int)n.user_id

            }).ToList();

            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFavourite(int? id,int ? idnews)
        {
            HttpCookie cookie = Request.Cookies["user_cookie"];
            User user = db.Users.Find(int.Parse(cookie.Value.ToString()));
            Favourite favourite = db.Favourites.Find(id);
            db.Favourites.Remove(favourite);
            db.SaveChanges();


            List<Favourite> Favourite = db.Favourites.Where(n => n.news_id == idnews && n.user_id == user.user_id).ToList();
            List<JsonFavourite> list = Favourite.Select(n => new JsonFavourite
            {
                datecreate = n.favourite_datecreate.ToString(),
                id = n.favourite_id,
                idnews = (int)n.news_id,
                idus = (int)n.user_id

            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}