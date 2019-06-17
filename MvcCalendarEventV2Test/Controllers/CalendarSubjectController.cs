using MvcCalendarEventV2Test.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize]
    public class CalendarSubjectController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarSubject Index
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index()
        {
            var csubject = db.CalendarSubjects.ToList();
            return View(csubject);
        }
        // GET: Subject Create
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: Subject Create
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Create(CalendarSubject csubject)
        {
            if (ModelState.IsValid)
            {
                db.CalendarSubjects.Add(csubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(csubject);
        }
        // Edit Subject
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var csubject = db.CalendarSubjects.SingleOrDefault(c => c.SubjectId == id);
            if (csubject == null)
            {
                return HttpNotFound();
            }
            return View(csubject);
        }
        // POST: Subject Edit
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Edit(CalendarSubject csubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(csubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(csubject);
        }
        // GET: Subject Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var csubject = db.CalendarSubjects.SingleOrDefault(c => c.SubjectId == id);
            if (csubject == null)
            {
                return HttpNotFound();
            }
            return View(csubject);
        }
        // POST: Subject Delete
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var csubject = db.CalendarSubjects.SingleOrDefault(c => c.SubjectId == id);
            db.CalendarSubjects.Remove(csubject ?? throw new InvalidOperationException());
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Dispose
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