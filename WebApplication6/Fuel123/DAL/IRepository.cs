//Интерфейс для определения структуры классов репозитариев для работы с данными конкретных таблиц -
//описывает набор необходимых методов - типовых операций с данными (CRUD)
using System;
using System.Collections.Generic;

namespace ToplivoCodeFirst.Models
{
    interface IRepository<T>:IDisposable where T:class
    {
        IEnumerable<T> GetAll();//получить коллекцию из всех объектов
        T Get(int id);//получить объект по индексу
        PagedCollection<T> GetNumberItems(Func<T, bool> predicate, int page = 1, int pageSize = 30);//получить коллекцию pageSize объектов расположенных на page странице и удовлетворяющих заданному условияю
        IEnumerable<T> Find(Func<T, bool> predicate);//получить коллекцию объектов, удовлетворяющих заданному условию
        void Create(T item);//создать объект
        void Delete(int id);//удалить объект
        void Update(T item);//обновить объект
        void Save(); // сохранение сделанных изменений

    }
}
