using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToplivoCodeFirst.Models;

namespace ToplivoCodeFirst.Controllers
{
    public class Tanks1Controller : Controller
    {
        private ToplivoContext db = new ToplivoContext();

        // GET: Tanks1
        public ActionResult Index()
        {
            return View(db.Tanks.Take(10).ToList());
        }

        // GET: Tanks1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = db.Tanks.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // GET: Tanks1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tanks1/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TankID,TankType,TankWeight,TankVolume,TankMaterial,TankPicture")] Tank tank)
        {
            if (ModelState.IsValid)
            {
                db.Tanks.Add(tank);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tank);
        }

        // GET: Tanks1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = db.Tanks.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // POST: Tanks1/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TankID,TankType,TankWeight,TankVolume,TankMaterial,TankPicture")] Tank tank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tank);
        }

        // GET: Tanks1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = db.Tanks.Find(id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // POST: Tanks1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tank tank = db.Tanks.Find(id);
            db.Tanks.Remove(tank);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
