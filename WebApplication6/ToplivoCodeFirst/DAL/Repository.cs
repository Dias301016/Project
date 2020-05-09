//Параметризованная версия реализации интерфейса IRepository<T> для объекта класса T,
//содержит описание методов для работы с данными таблицы, свзанной с объектом T
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ToplivoCodeFirst.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ToplivoContext db;
        //Конструктор класса
        public Repository(ToplivoContext context)
        {
            db = context;
        }
        //Создать новый объект
        public void Create(T item)
        {
            db.Set<T>().Add(item);
        }
        //Удалить объект
        public void Delete(int id)
        {
            T item = db.Set<T>().Find(id);
            if (item != null)
            {
                db.Set<T>().Remove(item);
            }
        }
        //Возвращает коллекцию объектов, удовлетворяющих заданному условию
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return db.Set<T>().Where(predicate).ToList();
        }
        //Возвращает один объект, выбранный по заданному ключу
        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }
        //Возвращает коллкцию из всех объектов 
        public IEnumerable<T> GetAll()
        {
            return db.Set<T>();
        }
        //Возвращает коллекцию объектов, удовлетворяющих заданному условию, для размещения на странице заданного размера и номера
        public PagedCollection<T> GetNumberItems(Func<T, bool> predicate, int page = 1, int pageSize = 30)
        {
            IEnumerable<T> items = db.Set<T>().Where(predicate).OrderBy(t => t);
            int totalitems = items.Count();
            if ((int)Math.Ceiling((decimal)totalitems / totalitems) < page) { page = 1; };
            //Если параметр page=0 разбиения будут возвращаться все объекты
            if (page != 0)
            {
                items = items.Skip((page - 1) * pageSize).Take(pageSize);
            };
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollection<T> viewfuels = new PagedCollection<T> { PageInfo = pageInfo, PagedItems = items };
            return viewfuels;
        }
        //Сохранение сделанных изменений
        public void Save()
        {
            db.SaveChanges();
        }
        //Обновление объекта
        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~Repository() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion




    }
}