//Реализация интерфейса IRepository<Fuel> для объекта класса Fuel,
//содержит описание методов для работы с данными таблицы Fuels, связанной с объектом Fuel

using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace ToplivoCodeFirst.Models
{
    public class FuelRepository : IRepository<Fuel>
    {
        private ToplivoContext db;
        public FuelRepository(ToplivoContext context)
        {
            db = context;
        }
        public void Create(Fuel fuel)
        {
            db.Fuels.Add(fuel);
        }

        public void Delete(int id)
        {
            Fuel fuel = db.Fuels.Find(id);
            if (fuel!=null)
            {
                db.Fuels.Remove(fuel);
            }
        }
        
        public IEnumerable<Fuel> Find(Func<Fuel, bool> predicate)
        {
            return db.Fuels.Where(predicate).ToList();
        }

        public Fuel Get(int id)
        {
            return db.Fuels.Find(id);
        }

        public IEnumerable<Fuel> GetAll()
        {
            return db.Fuels;
        }

        public PagedCollection<Fuel> GetNumberItems(Func<Fuel, bool> predicate, int page = 1, int pageSize = 20)
        {
            IEnumerable<Fuel> fuels= db.Fuels.Where(predicate).OrderBy(o => o.FuelID);
            int totalitems = fuels.Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            if (page != 0) 
            {
                fuels = fuels.Skip((page - 1) * pageSize).Take(pageSize);
            };
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollection<Fuel> viewfuels = new PagedCollection<Fuel> { PageInfo = pageInfo, PagedItems = fuels };
            return viewfuels;
        }

        public void Update(Fuel fuel)
        {
            db.Entry(fuel).State=EntityState.Modified;
        }
        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
 
    }

}