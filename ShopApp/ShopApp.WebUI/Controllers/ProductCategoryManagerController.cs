﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.Core.Contracts;
using ShopApp.Core.Models;
using ShopApp.DataAccess.InMemory;

namespace ShopApp.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> context;
        public ProductCategoryManagerController(IRepository<ProductCategory> productCategoryContext)
        {
            context = productCategoryContext;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = context.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory productToEdit = context.Find(id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                else
                {
                    //context.Update(productToEdit);
                    productToEdit.Category = productCategory.Category;
                    
                    context.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(string id)
        {
            ProductCategory product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(string id)
        {
            ProductCategory productCategory = context.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}