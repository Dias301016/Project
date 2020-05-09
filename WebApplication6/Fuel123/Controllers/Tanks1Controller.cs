using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fuel123.Models;

namespace Fuel123.Controllers
{
    public class Tanks1Controller : Controller
    {
        private ToplivoContext db = new ToplivoContext();

      
        public ActionResult Index()
        {
            return View(db.Tanks.Take(10).ToList());
        }

       
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

   
        public ActionResult Create()
        {
            return View();
        }

      
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
