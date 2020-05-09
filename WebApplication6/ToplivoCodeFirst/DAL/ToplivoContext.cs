//Класс контекста, отвечающий за связь с базой данных

using System.Data.Entity;
using ToplivoCodeFirst.Models;
namespace ToplivoCodeFirst.Models
{

    public class ToplivoContext : DbContext
    {
        
        public ToplivoContext() : base("name=ToplivoContext")
        {
        }
        public virtual DbSet<Fuel> Fuels { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Tank> Tanks { get; set; }
    }
}