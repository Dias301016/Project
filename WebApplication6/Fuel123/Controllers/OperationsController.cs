using PagedList;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Fuel123.Models;

namespace Fuel123.Controllers
{
    public class OperationsController : Controller
    {
        
        int pageSize = 15;
        
        UnitOfWork unitOfWork;
        
        TransferData transferdata;
      
        public OperationsController()
        {
          
            unitOfWork = new UnitOfWork();

        }

      
        public ActionResult Index( string strTankTypeFind, string strFuelTypeFind, int page = 1)
        {
            transferdata = (TransferData)Session["TransferData"];


            if (strTankTypeFind == null) { strTankTypeFind=transferdata.strTankTypeFind ?? ""; };
            if (strFuelTypeFind == null) { strFuelTypeFind = transferdata.strFuelTypeFind ?? ""; };

            transferdata.OperationPage = page;
            transferdata.strTankTypeFind = strTankTypeFind;
            transferdata.strFuelTypeFind = strFuelTypeFind;
            Session["TransferData"] = transferdata;

            IEnumerable<Operation> operations = unitOfWork.Operations.Find(t => ((t.Tank.TankType.Contains(strTankTypeFind)))&(t.Fuel.FuelType.Contains(strFuelTypeFind)));

       
            return View(operations.ToPagedList(page, pageSize));
        }

        // GET: Operations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return RedirectToIndex();

            Operation operation = unitOfWork.Operations.Get((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        
        public ActionResult Create()
        {
            ViewBag.FuelID = new SelectList(unitOfWork.Fuels.GetAll(), "FuelID", "FuelType");
            ViewBag.TankID = new SelectList(unitOfWork.Tanks.GetAll(), "TankID", "TankType");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Operation operation)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Operations.Create(operation);
                unitOfWork.Operations.Save();
                return RedirectToAction("Index");
            }

            ViewBag.FuelID = new SelectList(unitOfWork.Fuels.GetAll(), "FuelID", "FuelType", operation.FuelID);
            ViewBag.TankID = new SelectList(unitOfWork.Tanks.GetAll(), "TankID", "TankType", operation.TankID);
            return View(operation);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return RedirectToIndex();

            Operation operation = unitOfWork.Operations.Get((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            ViewBag.FuelID = new SelectList(unitOfWork.Fuels.GetAll(), "FuelID", "FuelType", operation.FuelID);
            ViewBag.TankID = new SelectList(unitOfWork.Tanks.GetAll(), "TankID", "TankType", operation.TankID);
            return View(operation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Operation operation)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Operations.Update(operation);
                unitOfWork.Operations.Save();
                return RedirectToAction("Index");
            }
            ViewBag.FuelID = new SelectList(unitOfWork.Fuels.GetAll(), "FuelID", "FuelType", operation.FuelID);
            ViewBag.TankID = new SelectList(unitOfWork.Tanks.GetAll(), "TankID", "TankType", operation.TankID);
            return View(operation);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = unitOfWork.Operations.Get((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.Operations.Delete(id);
            unitOfWork.Operations.Save();
            return RedirectToAction("Index");
        }

        public ActionResult RedirectToIndex()
        {

            transferdata = (TransferData)Session["TransferData"];
            int page = transferdata.OperationPage;
            string strTankTypeFind = transferdata.strTankTypeFind;
            string strFuelTypeFind = transferdata.strFuelTypeFind;
            IEnumerable<Operation> operations = unitOfWork.Operations.Find(t => ((t.Tank.TankType.Contains(strTankTypeFind)) & ((t.Fuel.FuelType.Contains(strFuelTypeFind)))));

            return View("Index", operations.ToPagedList(page, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
