using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ToplivoCodeFirst.Models;

namespace ToplivoCodeFirst
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //Инициализация БД путем выполнения кода в классе инициализатора с использование методов EF
            Database.SetInitializer(new ToplivoDbInitializer());
            
            //Инициализация БД путем запуска SQL инструкции из файла FillDB.sql
            //Database.SetInitializer(new ToplivoDbInitializer_runSQL());

            using (var db = new ToplivoContext())
            {
                db.Database.Initialize(true);
            }
            
            
        }
    }
}
