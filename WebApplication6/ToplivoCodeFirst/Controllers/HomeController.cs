using System.Web.Mvc;
using ToplivoCodeFirst.Models;

namespace ToplivoCodeFirst.Controllers
{

    public class HomeController : Controller
    {
        //Объект для управления репозиториями
        UnitOfWork unitOfWork;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata = new TransferData { TankPage = 1, FuelPage = 1, OperationPage = 1, strTankTypeFind = "Цистерна", strFuelTypeFind = "Нефть" };
        //Конструктор контроллера
        public HomeController()
        {
            // создаем экземпляр класса UnitOfWork, через свойства которого получим доступ к репозитариям 
            unitOfWork = new UnitOfWork();

        }

        public ActionResult Index(int pagesize = 10)
        {
            //Инициализация временных переменных сессии для использования разными объектами
            Session["TransferData"] = transferdata;
            int page = transferdata.OperationPage;
            ViewBag.NumberOperations = pagesize;
            //Получаем из БД  pagesize объектов Operation, при этом будут подгружаться данные из Tank и Fuel
            PagedCollection<Operation> pagedcollection = unitOfWork.Operations.GetNumberItems(t=>true,page, pagesize);
            // передаем все объекты в динамическое свойство Operations в ViewBag
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