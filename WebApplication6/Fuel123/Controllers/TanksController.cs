using System.Net;
using System.Web;
using System.Web.Mvc;
using Fuel123.Models;

namespace Fuel123.Controllers
{
    public class TanksController : Controller
    {
        
        UnitOfWork unitOfWork;
      
        TransferData transferdata;
      
        public TanksController()
        {
           
            unitOfWork = new UnitOfWork();

        }


          
        public ActionResult Index(PageInfo pageinfo)
        {

            transferdata = (TransferData)Session["TransferData"];

            int page = pageinfo.PageNumber; string strsearch = pageinfo.SearchString ?? "";

            transferdata.TankPage =page ; transferdata.strTankTypeFind = strsearch;
            Session["TransferData"] = transferdata;

            PagedCollection<Tank> pagedcollection = unitOfWork.Tanks.GetNumberItems(t => (t.TankType.Contains(strsearch)), page);
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
            Tank tank = unitOfWork.Tanks.Get((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

       
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tank tank, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string filename=unitOfWork.Tanks.CreateWithPicture(tank, upload);
                unitOfWork.Tanks.Save();
                return RedirectToAction("Edit", new { id = tank.TankID });
            }

            return View(tank);
        }

    
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return RedirectToIndex();

            Tank tank = unitOfWork.Tanks.Get((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }

            return View(tank);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tank tank, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload == null)
                {
                    unitOfWork.Tanks.Update(tank);
                }
                else
                {
                    string filename = unitOfWork.Tanks.UpdateWithPicture(tank, upload);
                }
                unitOfWork.Tanks.Save();
                
            }
            return View(tank);
        }

    
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = unitOfWork.Tanks.Get((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tank tank = unitOfWork.Tanks.Get((int)id);
            unitOfWork.Tanks.Delete(id);
            unitOfWork.Tanks.Save();
            return RedirectToAction("Index");
        }


        public ActionResult RedirectToIndex()
        {
            transferdata =(TransferData)Session["TransferData"];
            int page = transferdata.TankPage;
            string searchstring = transferdata.strTankTypeFind??"";
            PagedCollection<Tank> pagedcollection = unitOfWork.Tanks.GetNumberItems(t => (t.TankType.Contains(searchstring)),page);
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
