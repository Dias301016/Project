using System.Net;
using System.Web;
using System.Web.Mvc;
using Fuel123.Models;

namespace Fuel123.Controllers
{
    public class FuelsController : Controller
    {
       
        UnitOfWork unitOfWork;
        
        TransferData transferdata;
        
        public FuelsController()
        {
            
            unitOfWork = new UnitOfWork();

        }
        
        public ActionResult Index(PageInfo pageinfo)
        {
            transferdata = (TransferData)Session["TransferData"];


            int page = pageinfo.PageNumber; string strsearch = pageinfo.SearchString ?? "";

            transferdata.FuelPage = page; transferdata.strFuelTypeFind = strsearch;
            Session["TransferData"] = transferdata;

            PagedCollection<Fuel> pagedcollection = unitOfWork.Fuels.GetNumberItems(t => (t.FuelType.Contains(strsearch)), page);
            pagedcollection.PageInfo.SearchString = strsearch;

            return View(pagedcollection);
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return RedirectToIndex();

            Fuel fuel = unitOfWork.Fuels.Get((int)id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

      
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fuel fuel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Fuels.Create(fuel);
                unitOfWork.Fuels.Save();
                return RedirectToAction("Edit", new { id = fuel.FuelID });
            }

            return View(fuel);

        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id == -1) return RedirectToIndex();

            Fuel fuels = unitOfWork.Fuels.Get((int)id);
            if (fuels == null)
            {
                return HttpNotFound();
            }
            return View(fuels);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Fuel fuel)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Fuels.Update(fuel);
                unitOfWork.Fuels.Save();
                return RedirectToAction("Edit", new { id = fuel.FuelID });
            }
            return View(fuel);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = unitOfWork.Fuels.Get((int)id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.Fuels.Delete(id);
            unitOfWork.Fuels.Save();
            return RedirectToAction("Index");
        }

        public ActionResult RedirectToIndex()
        {
            transferdata = (TransferData)Session["TransferData"];
            int page = transferdata.FuelPage;
            string searchstring = transferdata.strFuelTypeFind;
            PagedCollection<Fuel> pagedcollection = unitOfWork.Fuels.GetNumberItems(t => (t.FuelType.Contains(searchstring)), page);
            pagedcollection.PageInfo.SearchString = searchstring;

            return View("Index", pagedcollection);
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
