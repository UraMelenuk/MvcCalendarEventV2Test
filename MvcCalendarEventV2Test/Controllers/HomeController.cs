using MvcCalendarEventV2Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "user,assistant,admin")]
        public ActionResult Index()
        {
            List<CalendarForm> listForm = db.CalendarForms.ToList();
            List<CalendarClassRoom> listClassRoom = db.CalendarClassRooms.ToList();
            List<CalendarGroup> listGroup = db.CalendarGroups.ToList();
            List<CalendarSubject> listSubject = db.CalendarSubjects.ToList();
            List<CalendarTeacher> listTeacher = db.CalendarTeachers.ToList();

            ViewBag.FormList = new SelectList(listForm, "FormId", "FormName");
            ViewBag.ClassRoomList = new SelectList(listClassRoom, "ClassRoomId", "ClassRoomName");
            ViewBag.GroupList = new SelectList(listGroup, "GroupId", "GroupName");
            ViewBag.SubjectList = new SelectList(listSubject, "SubjectId", "SubjectName");
            ViewBag.TeacherList = new SelectList(listTeacher, "TeacherId", "TeacherName");

            return View();
            
        }


        [Authorize(Roles ="user,assistant,admin")]
        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<EventViewModel> listEve = db.Events.Select(x => new EventViewModel
                {
                    EventID = x.EventID,

                    FormId = x.CalendarForm.FormId,
                    FormName = x.CalendarForm.FormName,
                    ClassRoomId = x.CalendarClassRoom.ClassRoomId,
                    ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                    GroupId = x.CalendarGroup.GroupId,
                    GroupName = x.CalendarGroup.GroupName,
                    SubjectId = x.CalendarSubject.SubjectId,
                    SubjectName = x.CalendarSubject.SubjectName,
                    TeacherId = x.CalendarTeacher.TeacherId,
                    TeacherName = x.CalendarTeacher.TeacherName,

                    Description = x.Description,
                    Start = x.Start,
                    End = x.End.Value,
                    ThemeColor = x.ThemeColor,
                    IsFullDay = x.IsFullDay
                }).ToList();

                return new JsonResult { Data = listEve, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [Authorize(Roles ="assistant,admin")]
        [HttpPost]
        public JsonResult SaveEvent(EventViewModel model)
        {
            var status = false;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    if (model.EventID > 0)
                    {
                        // Update Event
                        Event ev = db.Events.SingleOrDefault(x => x.EventID == model.EventID);

                        ev.FormId = model.FormId;
                        ev.ClassRoomId = model.ClassRoomId;
                        ev.GroupId = model.GroupId;
                        ev.SubjectId = model.SubjectId;
                        ev.TeacherId = model.TeacherId;

                        ev.Start = model.Start;
                        ev.End = model.End;
                        ev.IsFullDay = model.IsFullDay;
                        ev.ThemeColor = model.ThemeColor;
                        ev.Description = model.Description;

                    }
                    else
                    {
                        // Insert Event
                        Event ev = new Event
                        {
                            FormId = model.FormId,
                            ClassRoomId = model.ClassRoomId,
                            GroupId = model.GroupId,
                            SubjectId = model.SubjectId,
                            TeacherId = model.TeacherId,
                            Start = model.Start,
                            End = model.End,
                            IsFullDay = model.IsFullDay,
                            ThemeColor = model.ThemeColor,
                            Description = model.Description
                        };

                        int textEventId = model.EventID;

                        db.Events.Add(ev);
                    }

                    db.SaveChanges();
                    status = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            
            return new JsonResult { Data = new { status = status } };
        }


        [Authorize(Roles ="admin")]
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Event ev = db.Events.SingleOrDefault(x => x.EventID == eventID);

                if (ev != null)
                {
                    db.Events.Remove(ev);
                    db.SaveChanges();
                    status = true;
                }
            }


            return new JsonResult { Data = new { status = status } };
        }




    }
}