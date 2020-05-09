using System.Data.Entity;
using Fuel123.Models;
namespace Fuel123.Models
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