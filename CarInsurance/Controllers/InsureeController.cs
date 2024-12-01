using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Admin Section
        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public class InsureeController : Controller
        {
            private InsuranceEntities db = new InsuranceEntities();

            // POST: Insuree/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,Age,CarYear,CarMake,CarModel,SpeedingTickets,HasDUI,CoverageType")] Insuree insuree)
            {
                if (ModelState.IsValid)
                {
                    // Base monthly rate
                    decimal quote = 50m;

                    // Age logic
                    if (insuree.Age <= 18)
                    {
                        quote += 100;
                    }
                    else if (insuree.Age >= 19 && insuree.Age <= 25)
                    {
                        quote += 50;
                    }
                    else if (insuree.Age > 25)
                    {
                        quote += 25;
                    }

                    // Car year logic
                    if (insuree.CarYear < 2000)
                    {
                        quote += 25;
                    }
                    else if (insuree.CarYear > 2015)
                    {
                        quote += 25;
                    }

                    // Car make and model logic
                    if (insuree.CarMake.ToLower() == "porsche")
                    {
                        quote += 25;

                        if (insuree.CarModel.ToLower() == "911 carrera")
                        {
                            quote += 25;
                        }
                    }

                    // Speeding tickets
                    quote += insuree.SpeedingTickets * 10;

                    // DUI logic
                    if (insuree.HasDUI)
                    {
                        quote *= 1.25m;
                    }

                    // Coverage type logic
                    if (insuree.CoverageType)
                    {
                        quote *= 1.50m;
                    }

                    insuree.Quote = quote;

                    db.Insurees.Add(insuree);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(insuree);
            }
        }


        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateofBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateofBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
