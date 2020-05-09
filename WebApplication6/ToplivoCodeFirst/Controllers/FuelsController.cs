using System.Net;
using System.Web;
using System.Web.Mvc;
using ToplivoCodeFirst.Models;

namespace ToplivoCodeFirst.Controllers
{
    public class FuelsController : Controller
    {
        //Объект для управления репозиториями
        UnitOfWork unitOfWork;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        //Конструктор контроллера
        public FuelsController()
        {
            // создаем экземпляр класса UnitOfWork, через свойства которого получим доступ к репозитариям 
            unitOfWork = new UnitOfWork();

        }
        // GET: Fuels
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

        // GET: Fuels/Details/5
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

        // GET: Fuels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fuels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Fuels/Edit/5
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

        // POST: Fuels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Fuels/Delete/5
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

        // POST: Fuels/Delete/5
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
