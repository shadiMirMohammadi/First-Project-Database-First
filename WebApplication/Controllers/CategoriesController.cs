using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CategoriesController : Controller
    {
        DB_WebEntities dbCategories = new DB_WebEntities();
        public ActionResult Index()
        {
            return View(dbCategories.T_Categories);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "CategoryName")] T_Categories category)
        {
            if (ModelState.IsValid)
            {
                category.RegisterAdminId = 13;
                dbCategories.T_Categories.Add(category);
                dbCategories.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbCategories.T_Categories.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbCategories.T_Categories.Find(id));
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, CategoryName, RegisterAdminId")] T_Categories category)
        {
            if (ModelState.IsValid)
            {
                dbCategories.Entry(category).State = EntityState.Modified;
                dbCategories.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}