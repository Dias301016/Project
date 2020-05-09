//Класс для представление данных из класса T, разбитыми на порции - страницы
using System.Collections.Generic;


namespace ToplivoCodeFirst.Models
{
    public class PagedCollection<T>
    {
        public IEnumerable<T> PagedItems { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}