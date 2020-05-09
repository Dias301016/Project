//Класс содержит описание структуры порции предстваления данных - страницы, с котрой работает пользователь приложения.
using System;

namespace ToplivoCodeFirst.Models
{
   
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public string SearchString { get; set; } //значение строки поиска 
        public int TotalPages  // всего страниц
        {
            get {
                if (PageSize== 0) return 0;
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }
        public PageInfo()
        {
            PageNumber = 1;
            PageSize = 20;
            TotalItems = 0;
        }
    }
}