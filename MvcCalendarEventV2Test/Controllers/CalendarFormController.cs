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
    public class CalendarFormController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarForm Index
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index()
        {
            var cforms = db.CalendarForms.ToList();
            return View(cforms);
        }
        // GET: Form Create
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: Form Create
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Create(CalendarForm cform)
        {
            if (ModelState.IsValid)
            {
                db.CalendarForms.Add(cform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cform);
        }
        // Edit Form
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cform = db.CalendarForms.SingleOrDefault(c => c.FormId == id);
            if(cform == null)
            {
                return HttpNotFound();
            }
            return View(cform);
        }
        // POST: Form Edit
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Edit(CalendarForm cform)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cform);
        }
        // GET: Form Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cform = db.CalendarForms.SingleOrDefault(c => c.FormId == id);
            if(cform == null)
            {
                return HttpNotFound();
            }
            return View(cform);
        }
        // POST: Form Delete
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cform = db.CalendarForms.SingleOrDefault(c => c.FormId == id);
            db.CalendarForms.Remove(cform ?? throw new InvalidOperationException());
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Dispose
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}