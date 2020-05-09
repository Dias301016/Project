using System.Web.Mvc;
using Fuel123.Models;

namespace Fuel123.Controllers
{

    public class HomeController : Controller
    {
    
        UnitOfWork unitOfWork;      
        TransferData transferdata = new TransferData { TankPage = 1, FuelPage = 1, OperationPage = 1, strTankTypeFind = "Цистерна", strFuelTypeFind = "Нефть" };
        
        public HomeController()
        {
            
            unitOfWork = new UnitOfWork();

        }

        public ActionResult Index(int pagesize = 10)
        {
           
            Session["TransferData"] = transferdata;
            int page = transferdata.OperationPage;
            ViewBag.NumberOperations = pagesize;
           
            PagedCollection<Operation> pagedcollection = unitOfWork.Operations.GetNumberItems(t=>true,page, pagesize);
          
            ViewBag.Operations = pagedcollection.PagedItems;
            ViewBag.PageInfo = pagedcollection.PageInfo;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Топливная база";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}