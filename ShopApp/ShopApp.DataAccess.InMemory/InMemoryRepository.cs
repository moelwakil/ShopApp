using ShopApp.Core.Contracts;
using ShopApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);
            if (tToUpdate == null)
            {
                throw new Exception(className + " Not Found");
            }
            else
            {
                tToUpdate = t;
            }
        }

        public T Find(string id)
        {
            T t = items.Find(i => i.Id == id);
            if (t == null)
            {
                throw new Exception(className + " Not Found");
            }
            else
            {
                return t;
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string id)
        {
            T t = items.Find(i => i.Id == id);
            if (t == null)
            {
                throw new Exception(className + " Not Found");
            }
            else
            {
                items.Remove(t);
            }
        }
    }
}
