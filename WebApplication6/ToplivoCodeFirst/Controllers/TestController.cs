using System.Collections.Generic;
using System.Web.Mvc;
using ToplivoCodeFirst.Models;
using System.Data.Entity;
using System.Linq;

namespace ToplivoCodeFirst.Controllers
{
    public class TestController : Controller
    {
        ToplivoContext db = new ToplivoContext();
        // GET: Test
        public ActionResult Index()
        {
            IEnumerable<Operation> operations = db.Operations.Include(t=>t.Tank).Include(t=>t.Fuel).Take(10);
        

            return View(operations);
        }
        public string Hello(string name)
        {
            return "Меня зовут " + name;
        }
    }
}