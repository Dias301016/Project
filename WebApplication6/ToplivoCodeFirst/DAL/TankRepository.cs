//Реализация интерфейса IRepository<Operation> для объекта класса Tank,
//содержит описание методов для работы с данными таблицы Tanks, связанной с объектом Tank
using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;

namespace ToplivoCodeFirst.Models
{
    public class TankRepository : IRepository<Tank>
    {
        private ToplivoContext db;
        public TankRepository(ToplivoContext context)
        {
            db = context;
        }

        public void Create(Tank tank)
        {
            db.Tanks.Add(tank);
        }
        public string CreateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";  
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Images в приложении
                upload.SaveAs(HttpContext.Current.Server.MapPath(fileName));
            }
            tank.TankPicture = fileName;
            Create(tank);
            return fileName;

        }
        public string UpdateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";  
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Images в приложении
                upload.SaveAs(HttpContext.Current.Server.MapPath(fileName));
                tank.TankPicture = fileName;
            }
            Update(tank);
            return fileName;
        }

        public void Delete(int id)
        {
            Tank tank = db.Tanks.Find(id);
            if (tank != null)
            {
                db.Tanks.Remove(tank);
            }
        }

        public IEnumerable<Tank> Find(Func<Tank, bool> predicate)
        {
            return db.Tanks.Where(predicate).ToList();
        }

        public Tank Get(int id)
        {
            return db.Tanks.Find(id);
        }

        public IEnumerable<Tank> GetAll()
        {
            return db.Tanks;
        }

       
        public PagedCollection<Tank> GetNumberItems(Func<Tank, bool> predicate, int page = 1, int pageSize = 20)
        {
            int totalitems = db.Tanks.Where(predicate).Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            IEnumerable<Tank> tanks = db.Tanks.Where(predicate).OrderBy(o => o.TankID).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollection<Tank> viewtanks = new PagedCollection<Tank> { PageInfo = pageInfo, PagedItems = tanks };
            return viewtanks;
        }

        public void Update(Tank tank)
        {
            db.Entry(tank).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
