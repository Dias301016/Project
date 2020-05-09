using System.Net;
using System.Web;
using System.Web.Mvc;
using ToplivoCodeFirst.Models;

namespace ToplivoCodeFirst.Controllers
{
    public class TanksController : Controller
    {
        //Объект для управления репозиториями
        UnitOfWork unitOfWork;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        //Конструктор контроллера
        public TanksController()
        {
            // создаем экземпляр класса UnitOfWork, через свойства которого получим доступ к репозитариям 
            unitOfWork = new UnitOfWork();

        }


        // GET: Tanks        
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

        // GET: Tanks/Details/5
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

        // GET: Tanks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tanks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Tanks/Edit/5
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

        // POST: Tanks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Tanks/Delete/5
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

        // POST: Tanks/Delete/5
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
