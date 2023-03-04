using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using System.IO;

namespace WebApplication.Controllers
{
    public class AdminsController : Controller
    {
        DB_WebEntities dbAdmins = new DB_WebEntities();

        #region Index
        public ActionResult Index()
        {
            return View(dbAdmins.T_Admins);
        }
        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Create
        [HttpGet] 
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Family, PhoneNumber, Password, Email, NationalCode, ImageName")] T_Admins admin, HttpPostedFileBase ImageName)
        {
            if (ModelState.IsValid)
            {
                string newImageName = "user.jpg";

                if (ImageName != null)
                {
                    //فیلم بفرسته با فرمت mp4 باگ داره

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
                    ImageName.SaveAs(Server.MapPath("/Image/Profile/" + newImageName));
                }
                admin.ImageName = newImageName;
                admin.RoleId = 1;
                admin.RegisterDate = DateTime.Now;
                admin.IsActive = true;
                dbAdmins.T_Admins.Add(admin);
                dbAdmins.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (dbAdmins.T_Admins.Find(id)==null)
            {
                return HttpNotFound();
            }
            return View(dbAdmins.T_Admins.Find(id));
        }
        #endregion

    }
}