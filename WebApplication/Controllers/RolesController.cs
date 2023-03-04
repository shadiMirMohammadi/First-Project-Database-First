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
    public class RolesController : Controller
    {
        DB_WebEntities dbRoles = new DB_WebEntities();
        #region Index
        public ActionResult Index()
        {
            return View(dbRoles.T_Roles);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "RoleName,RoleTitle")] T_Roles role)
        {
            if (ModelState.IsValid)
            {
                dbRoles.T_Roles.Add(role);
                dbRoles.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
            if (dbRoles.T_Roles.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbRoles.T_Roles.Find(id));
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "RoleId, RoleName, RoleTitle")] T_Roles role)
        {
            if (ModelState.IsValid)
            {
                dbRoles.Entry(role).State = EntityState.Modified;
                dbRoles.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
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
            if (dbRoles.T_Roles.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbRoles.T_Roles.Find(id));
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbRoles.T_Roles.Find(id) == null)
            {
                return HttpNotFound();
            }
            var role = dbRoles.T_Roles.Find(id);
            if (role != null)
            {
                dbRoles.T_Roles.Remove(role);
                dbRoles.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }
        #endregion



    }
}