using System.Collections.Generic;


namespace Fuel123.Models
{
    public class PagedCollection<T>
    {
        public IEnumerable<T> PagedItems { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}