using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace WebApplication.Controllers
{
    public class ProductsController : Controller
    {
        DB_WebEntities dbProducts = new DB_WebEntities();

        #region Index
        public ActionResult Index()
        {
            return View(dbProducts.T_Products);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[CategoryName], [BrandName], [ModelName], [Description], [Price], [Inventory], [SalesNumber], [RegisterDate], [IsActive], [InventoryStatus], [RegisterAdminId], [CategoryId], [ImageName]
        public ActionResult Create([Bind(Include = "CategoryName, BrandName, ModelName, Description, Price, Inventory, ImageName")] T_Products product, HttpPostedFileBase ImageName)
        {
            if (ModelState.IsValid)
            {
                string newImageName = "";

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
                    ImageName.SaveAs(Server.MapPath("/Image/Product/ProductProfile/" + newImageName));
                }
                product.ImageName = newImageName;

                //List<T_Categories> categoryList = new List<T_Categories>();
                //if (!categoryList.Any(t => t.CategoryName == product.CategoryName))
                //{
                //    ModelState.AddModelError("CategoryName", "دسته بندی مورد نظر وجود ندارد");
                //    return View();
                //}
                //if (product.CategoryName.Trim() == "آرایش صورت")
                //{
                //    product.CategoryId = 1;
                //}
                //else if (product.CategoryName.Trim() == "آرایش چشم")
                //{
                //    product.CategoryId = 2;
                //}
                //else if (product.CategoryName.Trim() == "آرایش ابرو")
                //{
                //    product.CategoryId = 3;
                //}
                //else if (product.CategoryName.Trim() == "آرایش ناخن")
                //{
                //    product.CategoryId = 4;
                //}
                //else if (product.CategoryName.Trim() == "آرایش لب")
                //{
                //    product.CategoryId = 5;
                //}
                //else if (product.CategoryName.Trim() == "لوازم جانبی")
                //{
                //    product.CategoryId = 6;
                //}
                //else if (product.CategoryName.Trim() == " براش آرایشی")
                //{
                //    product.CategoryId = 7;
                //}

                product.CategoryId = 13;
                product.RegisterAdminId = 13;
                product.InventoryStatus = true;
                product.IsActive = true;
                product.RegisterDate = DateTime.Now;
                product.SalesNumber = 0;
                dbProducts.T_Products.Add(product);
                dbProducts.SaveChanges();
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
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (dbProducts.T_Products.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbProducts.T_Products.Find(id));
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
            if (dbProducts.T_Products.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbProducts.T_Products.Find(id));
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, CategoryName, BrandName, ModelName, Description, Price, Inventory, SalesNumber, RegisterDate, IsActive, InventoryStatus, RegisterAdminId, CategoryId, ImageName")] T_Products product , HttpPostedFileBase newImageProduct)
        {
            if (ModelState.IsValid)
            {

                if (newImageProduct != null)
                {
                    //فیلم بفرسته با فرمت mp4 باگ داره
                    if (newImageProduct.ContentType != "image/jpeg" && newImageProduct.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageName", "تصویر شما باید با فرمت png یا jpg یا jpeg باشد");
                        return View(product);
                    }
                    if (newImageProduct.ContentLength > 300000)
                    {
                        ModelState.AddModelError("ImageName", "سایز تصویر شما نباید بیشتر از 300 کیلوبایت باشد");
                        return View(product);
                    }
                    string newImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newImageProduct.FileName);
                    newImageProduct.SaveAs(Server.MapPath("/Image/Product/ProductProfile/" + newImageName));
                    product.ImageName = newImageName;
                }

                //if (product.CategoryName.Trim() == "آرایش صورت")
                //{
                //    product.CategoryId = 1;
                //}
                //else if (product.CategoryName.Trim() == "آرایش چشم")
                //{
                //    product.CategoryId = 2;
                //}
                //else if (product.CategoryName.Trim() == "آرایش ابرو")
                //{
                //    product.CategoryId = 3;
                //}
                //else if (product.CategoryName.Trim() == "آرایش ناخن")
                //{
                //    product.CategoryId = 4;
                //}
                //else if (product.CategoryName.Trim() == "آرایش لب")
                //{
                //    product.CategoryId = 5;
                //}
                //else if (product.CategoryName.Trim() == "لوازم جانبی")
                //{
                //    product.CategoryId = 6;
                //}
                //else if (product.CategoryName.Trim() == " براش آرایشی")
                //{
                //    product.CategoryId = 7;
                //}


                product.CategoryId = 13;
                dbProducts.Entry(product).State = EntityState.Modified;
                dbProducts.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
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
            if (dbProducts.T_Products.Find(id) == null)
            {
                return HttpNotFound();
            }
            return View(dbProducts.T_Products.Find(id));
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (dbProducts.T_Products.Find(id) == null)
            {
                return HttpNotFound();
            }
            var product = dbProducts.T_Products.Find(id);
            if (product != null)
            {
                dbProducts.T_Products.Remove(product);


                if (product.ImageName != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Image/Product/ProfileUser") + product.ImageName))
                    {
                        System.IO.File.Delete(Server.MapPath("/Image/Product/ProductProfile" + product.ImageName));
                    }
                }
                dbProducts.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }


        #endregion






    }
}