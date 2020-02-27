using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopApp.Core.Contracts;
using ShopApp.Core.Models;
using ShopApp.Core.ViewModels;
using ShopApp.DataAccess.InMemory;

namespace ShopApp.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create (Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            } else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit (string id)
        {
            Product product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            } else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit (Product product, string id)
        {
            Product productToEdit = context.Find(id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            } else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                } else
                {
                    //context.Update(productToEdit);
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete (string id)
        {
            Product product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            } else
            {
                return View(product);
            }
        }

        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete (string id)
        {
            Product product = context.Find(id);
            if (product == null)
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