using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using ShopApp.Core.Models;

namespace ShopApp.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> categories;

        public ProductCategoryRepository()
        {
            categories = cache["categories"] as List<ProductCategory>;
            if (categories == null)
            {
                categories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["categories"] = categories;
        }

        public void Insert(ProductCategory pc)
        {
            categories.Add(pc);
        }

        public void Update (ProductCategory pc)
        {
            ProductCategory pcToUpdate = categories.Find(p => p.Id == pc.Id);
            if (pcToUpdate == null)
            {
                throw new Exception("Product category not found");
            } else
            {
                pcToUpdate = pc;
            }
        }

        public ProductCategory Find (string id)
        {
            ProductCategory p = categories.Find(pp => pp.Id == id);
            if (p == null)
            {
                throw new Exception("Product category not found");
            }
            else
            {
                return p;
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return categories.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductCategory p = categories.Find(pp => pp.Id == id);
            if (p == null)
            {
                throw new Exception("Product category not found");
            } else
            {
                categories.Remove(p);
            }
        }
    }
}
