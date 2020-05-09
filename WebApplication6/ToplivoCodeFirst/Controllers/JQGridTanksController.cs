//Контроллер для работы с данными таблицы Tanks с помощью jqGrid плагина JavaScript библиотеки jQuery
using ToplivoCodeFirst.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ToplivoCodeFirst.Controllers
{
    public class JQGridTanksController : Controller
    {
        //Объект для управления репозиториями
        UnitOfWork unitOfWork;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        //Конструктор контроллера
        public JQGridTanksController()
        {
            // создаем экземпляр класса UnitOfWork, через свойства которого получим доступ к репозитариям 
            unitOfWork = new UnitOfWork();

        }
        // GET: /JQGridTanks/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTanks(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            transferdata.FuelPage = page; transferdata.strFuelTypeFind = searchString;
            Session["TransferData"] = transferdata;
            sord = (sord == null) ? "" : sord;

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var tanks = unitOfWork.Tanks.GetAll().Select(
                    t => new
                    {
                        t.TankID,
                        t.TankType,
                        t.TankVolume,
                        t.TankWeight,
                        t.TankMaterial,
                        t.TankPicture
                    });
            if (_search)
            {
                switch (searchField)
                {
                    case "TankType":
                        tanks = tanks.Where(t => t.TankType.Contains(searchString));
                        break;
                    case "TankMaterial":
                        tanks = tanks.Where(t => t.TankMaterial.Contains(searchString));
                        break;                                       
                }
            }
            int totalRecords = tanks.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            string sortOrder = sidx + " " + sord.ToUpper();
            switch (sortOrder)
            {
                case "TankMaterial ASC":
                    tanks = tanks.OrderBy(s => s.TankMaterial);
                    break;
                case "TankType ASC":
                    tanks = tanks.OrderBy(s => s.TankType);
                    break;
                case "TankMaterial DESC":
                    tanks = tanks.OrderByDescending(s => s.TankMaterial);
                    break;
                default:
                    tanks = tanks.OrderByDescending(s => s.TankType);
                    break;
            }
            
            tanks = tanks.Skip(pageIndex * pageSize).Take(pageSize);


            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = tanks
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Create([Bind(Exclude = "TankID")] Tank Model)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Tanks.Create(Model);
                    unitOfWork.Tanks.Save();
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Ошибка валидации";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }
        public string Edit(Tank Model)
        {
            string msg;

            try
            {
                if (ModelState.IsValid)
                {
 
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Ошибка валидации";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }
        public string Delete(string Id)
        {
            unitOfWork.Tanks.Delete(Convert.ToInt32(Id));
            unitOfWork.Tanks.Save();
            return "Удалено";
        }
        [HttpPost]
        public string Upload(Tank tank)
        {
            string fileName="";

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
 
                    fileName = unitOfWork.Tanks.UpdateWithPicture(tank, upload);
                    unitOfWork.Tanks.Save();
                    return "файл загружен";

                }

            }
            unitOfWork.Tanks.Update(tank);
            unitOfWork.Tanks.Save();
            return "данные обновлены";
        }



    }
}