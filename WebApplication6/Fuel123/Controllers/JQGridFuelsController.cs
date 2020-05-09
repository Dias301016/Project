
using System;
using System.Linq;
using System.Web.Mvc;
using Fuel123.Models;
using System.Data.Entity;

namespace Fuel123.Controllers
{
    public class JQGridFuelsController : Controller
    {
      
        UnitOfWork unitOfWork;
       
        TransferData transferdata;
       
        public JQGridFuelsController()
        {
           
            unitOfWork = new UnitOfWork();

        }

        
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