//Контроллер для работы с данными таблицы Fuels с помощью jqGrid плагина JavaScript библиотеки jQuery
using System;
using System.Linq;
using System.Web.Mvc;
using ToplivoCodeFirst.Models;
using System.Data.Entity;

namespace ToplivoCodeFirst.Controllers
{
    public class JQGridFuelsController : Controller
    {
        //Объект для управления репозиториями
        UnitOfWork unitOfWork;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        //Конструктор контроллера
        public JQGridFuelsController()
        {
            // создаем экземпляр класса UnitOfWork, через свойства которого получим доступ к репозитариям 
            unitOfWork = new UnitOfWork();

        }

        // GET: JQGridFuels
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetFuels(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            transferdata.FuelPage = page; transferdata.strFuelTypeFind = searchString;
            Session["TransferData"] = transferdata;
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var fuels = unitOfWork.Fuels.GetAll().Select(
                    t => new
                    {
                        t.FuelID,
                        t.FuelType,
                        t.FuelDensity,
                    });

            if (_search)
            {
                switch (searchField)
                {
                    case "FuelType":
                        fuels = fuels.Where(t => t.FuelType.Contains(searchString));
                        break;
                    
                }
            }

            int totalRecords = fuels.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                fuels = fuels.OrderByDescending(t => t.FuelType);
                fuels = fuels.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                fuels = fuels.OrderBy(t => t.FuelType);
                fuels = fuels.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = fuels
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string Create([Bind(Exclude = "FuelID")] Fuel Model)
        {
            
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Fuels.Create(Model);
                    unitOfWork.Fuels.Save();
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Модель не прошла валидацию";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }
        public string Edit(Fuel Model)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Fuels.Update(Model);
                    unitOfWork.Fuels.Save();
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Модель не прошла валидацию";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }
        public string Delete(string id)
        {
            unitOfWork.Fuels.Delete(Convert.ToInt32(id));
            unitOfWork.Fuels.Save();
            return "Запись удалена";
        }

    }
}