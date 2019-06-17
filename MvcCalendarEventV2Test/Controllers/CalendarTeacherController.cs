using MvcCalendarEventV2Test.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize]
    public class CalendarTeacherController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarTeacher Index
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index()
        {
            var cteacher = db.CalendarTeachers.ToList();
            return View(cteacher);
        }
        // GET: Teacher Contact
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Contact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cteacher = db.CalendarTeachers.SingleOrDefault(c => c.TeacherId == id);
            if (cteacher == null)
            {
                return HttpNotFound();
            }
            return View(cteacher);
        }
        // POST: Teacher Contact Sending To Mail
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(CalendarTeacher model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message: {2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.TeacherEmail.ToString()));
                message.Subject = "Schedule File";
                message.Body = string.Format(body, "IT-STEP ACADEMY" /*model.TeacherName*/, "emailcalendarevent@gmail.com" /*model.TeacherEmail*/, model.TeacherMessage);
                message.IsBodyHtml = true;

                if (model.UploadFile != null && model.UploadFile.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(model.UploadFile.InputStream, Path.GetFileName(model.UploadFile.FileName)));
                }
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }
        // Sent
        public ActionResult Sent()
        {
            return View();
        }


        // GET: Teacher Create
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Create()
        {
            return View();
        }
        // POST: Teacher Create
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Create(CalendarTeacher cteacher)
        {
            if (ModelState.IsValid)
            {
                db.CalendarTeachers.Add(cteacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cteacher);
        }
        // Edit Teacher
        [Authorize(Roles = "assistant,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cteacher = db.CalendarTeachers.SingleOrDefault(c => c.TeacherId == id);
            if (cteacher == null)
            {
                return HttpNotFound();
            }
            return View(cteacher);
        }
        // POST: Teacher Edit
        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Edit(CalendarTeacher cteacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cteacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cteacher);
        }
        // GET: Teacher Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cteacher = db.CalendarTeachers.SingleOrDefault(c => c.TeacherId == id);
            if (cteacher == null)
            {
                return HttpNotFound();
            }
            return View(cteacher);
        }
        // POST: Teacher Delete
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cteacher = db.CalendarTeachers.SingleOrDefault(c => c.TeacherId == id);
            db.CalendarTeachers.Remove(cteacher ?? throw new InvalidOperationException());
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