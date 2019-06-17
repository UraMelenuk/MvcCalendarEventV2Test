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
    public class CalendarClassRoomController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarClassRoom Index
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index()
        {
            var cclassroom = db.CalendarClassRooms.ToList();
            return View(cclassroom);
        }

        // GET: ClassRoom Create
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: ClassRoom Create
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Create(CalendarClassRoom cclassroom)
        {
            if (ModelState.IsValid)
            {
                db.CalendarClassRooms.Add(cclassroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cclassroom);
        }

        // Edit ClassRoom
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cclassroom = db.CalendarClassRooms.SingleOrDefault(c => c.ClassRoomId == id);
            if (cclassroom == null)
            {
                return HttpNotFound();
            }
            return View(cclassroom);
        }
        // POST: ClassRoom Edit
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Edit(CalendarClassRoom cclassroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cclassroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cclassroom);
        }

        // GET: ClassRoom Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cclassroom = db.CalendarClassRooms.SingleOrDefault(c => c.ClassRoomId == id);
            if (cclassroom == null)
            {
                return HttpNotFound();
            }
            return View(cclassroom);
        }
        // POST: ClassRoom Delete
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cclassroom = db.CalendarClassRooms.SingleOrDefault(c => c.ClassRoomId == id);
            db.CalendarClassRooms.Remove(cclassroom ?? throw new InvalidOperationException());
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