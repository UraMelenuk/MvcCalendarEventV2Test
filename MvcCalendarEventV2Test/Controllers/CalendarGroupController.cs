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
    public class CalendarGroupController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarGroup Index
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index()
        {
            var cgroup = db.CalendarGroups.ToList();
            return View(cgroup);
        }
        // GET: Group Create
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: Group Create
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Create(CalendarGroup cgroup)
        {
            if (ModelState.IsValid)
            {
                db.CalendarGroups.Add(cgroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cgroup);
        }
        // Edit Group
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cgroup = db.CalendarGroups.SingleOrDefault(c => c.GroupId == id);
            if (cgroup == null)
            {
                return HttpNotFound();
            }
            return View(cgroup);
        }
        // POST: Group Edit
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Edit(CalendarGroup cgroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cgroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cgroup);
        }
        // GET: Group Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cgroup = db.CalendarGroups.SingleOrDefault(c => c.GroupId == id);
            if (cgroup == null)
            {
                return HttpNotFound();
            }
            return View(cgroup);
        }
        // POST: Group Delete
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cgroup = db.CalendarGroups.SingleOrDefault(c => c.GroupId == id);
            db.CalendarGroups.Remove(cgroup ?? throw new InvalidOperationException());
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