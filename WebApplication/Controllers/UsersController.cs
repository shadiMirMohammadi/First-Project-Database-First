using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using System.IO;
using System.Net;
using System.Data.Entity;
using GSD.Globalization;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        DB_WebEntities dbUsers = new DB_WebEntities();

        #region Index
        public ActionResult Index()
        {
            return View(dbUsers.T_Users);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Family, PhoneNumber, Password, Email, NationalCode,  ImageName")] T_Users user, HttpPostedFileBase ImageName)
        {
            if (ModelState.IsValid)
            {
                string newImageName = "user.jpg";

                if (ImageName != null)
                {

                    if (ImageName.ContentType != "image/jpeg" && ImageName.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageName", "تصویر شما باید با فرمت png یا jpg یا jpeg باشد");
                        return View();
                    }

                    if (ImageName.ContentLength > 300000)
                    {
                        ModelState.AddModelError("ImageName", "سایز تصویر شما نباید بیشتر از 300 کیلوبایت باشد");
                        return View();
                    }

                    newImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImageName.FileName);
                    ImageName.SaveAs(Server.MapPath("/Image/Profile/ProfileUser/" + newImageName));
                }
                user.ImageName = newImageName;
                user.RoleId = 2;
                user.RegisterDate = DateTime.Now;
                user.IsActive = true;
                dbUsers.T_Users.Add(user);
                dbUsers.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbUsers.T_Users.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbUsers.T_Users.Find(id));
        }
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbUsers.T_Users.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbUsers.T_Users.Find(id));
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, Family, PhoneNumber, Password, Email, NationalCode, RegisterDate, IsActive, ImageName, RoleId")] T_Users user, HttpPostedFileBase newImageProfile)
        {
            if (ModelState.IsValid)
            {
                if (newImageProfile != null)
                {
                    //فیلم بفرسته با فرمت mp4 باگ داره
                    if (newImageProfile.ContentType != "image/jpeg" && newImageProfile.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageName", "تصویر شما باید با فرمت png یا jpg یا jpeg باشد");
                        return View(user);
                    }
                    if (newImageProfile.ContentLength > 300000)
                    {
                        ModelState.AddModelError("ImageName", "سایز تصویر شما نباید بیشتر از 300 کیلوبایت باشد");
                        return View(user);
                    }
                    string newImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newImageProfile.FileName);
                    newImageProfile.SaveAs(Server.MapPath("/Image/Profile/ProfileUser/" + newImageName));
                    user.ImageName = newImageName;
                }
                dbUsers.Entry(user).State = EntityState.Modified;
                dbUsers.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        #endregion


        #region Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbUsers.T_Users.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbUsers.T_Users.Find(id));
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbUsers.T_Users.Find(id) == null)
            {
                return HttpNotFound();
            }
            var user = dbUsers.T_Users.Find(id);
            if (user != null)
            {
                dbUsers.T_Users.Remove(user);
                

                if (user.ImageName != "user.png")
                {
                    if (System.IO.File.Exists(Server.MapPath("/Image/Profile/ProfileUser") + user.ImageName))
                    {
                        System.IO.File.Delete(Server.MapPath("/Image/Profile/ProfileUser" + user.ImageName));
                    }
                }
                dbUsers.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }


        #endregion


    }
}