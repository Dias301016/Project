//Реализация интерфейса IRepository<Operation> для объекта класса Operation,
//содержит описание методов для работы с данными таблицы Operations, связанной с объектом Operation

using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace ToplivoCodeFirst.Models
{
    public class OperationRepository : IRepository<Operation>
    {
        private ToplivoContext db;
        public OperationRepository(ToplivoContext context)
        {
            db = context;
        }

        public void Create(Operation operation)
        {
            db.Operations.Add(operation);
        }

        public void Delete(int id)
        {
            Operation operation=db.Operations.Find(id);
            if (operation != null)
            {
                db.Operations.Remove(operation);
            }            
        }

        public IEnumerable<Operation> Find(Func<Operation, bool> predicate)
        {
            return db.Operations.Include(o => o.Fuel).Include(o => o.Tank).Where(predicate).ToList();
        }

        public Operation Get(int id)
        {
            return db.Operations.Find(id);
        }

        public IEnumerable<Operation> GetAll()
        {
            return db.Operations.Include(o=>o.Fuel).Include(o=>o.Tank).OrderBy(o=>o.OperationID);
        }

        public PagedCollection<Operation> GetNumberItems(Func<Operation, bool> predicate, int page = 1, int pageSize = 20)
        {

            int totalitems = db.Operations.Where(predicate).Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            IEnumerable<Operation> operations = db.Operations.OrderBy(o => o.OperationID).Skip((page - 1) * pageSize).Take(pageSize).Include(o => o.Fuel).Include(o => o.Tank).Where(predicate);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollection<Operation> viewoperations = new PagedCollection<Operation> { PageInfo = pageInfo, PagedItems = operations };
            return viewoperations;
        }
  
        public void Update(Operation operation)
        {
            db.Entry(operation).State=EntityState.Modified;
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