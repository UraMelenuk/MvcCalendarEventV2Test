using MvcCalendarEventV2Test.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize]
    public class CalendarEventController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "assistant,admin")]
        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "searchAll")
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

                List<EventViewModel> listEve = db.Events.Where(x => x.CalendarForm.FormName == search || x.CalendarClassRoom.ClassRoomName == search || x.CalendarGroup.GroupName == search || x.CalendarSubject.SubjectName == search || x.CalendarTeacher.TeacherName == search || search == null).Select(x => new EventViewModel
                {
                    EventID = x.EventID,
                    FormName = x.CalendarForm.FormName,
                    ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                    GroupName = x.CalendarGroup.GroupName,
                    SubjectName = x.CalendarSubject.SubjectName,
                    TeacherName = x.CalendarTeacher.TeacherName,

                    Description = x.Description,
                    Start = x.Start,
                    End = x.End.Value,
                    ThemeColor = x.ThemeColor,
                    IsFullDay = x.IsFullDay
                }).ToList();

                ViewBag.EventList = listEve;

                return View();
            }
            else
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

                List<EventViewModel> listEve = db.Events.Where(x => x.CalendarForm.FormName.StartsWith(search) || x.CalendarClassRoom.ClassRoomName.StartsWith(search) || x.CalendarGroup.GroupName.StartsWith(search) || x.CalendarSubject.SubjectName.StartsWith(search) || x.CalendarTeacher.TeacherName.StartsWith(search) || search == null).Select(x => new EventViewModel
                {
                    EventID = x.EventID,
                    FormName = x.CalendarForm.FormName,
                    ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                    GroupName = x.CalendarGroup.GroupName,
                    SubjectName = x.CalendarSubject.SubjectName,
                    TeacherName = x.CalendarTeacher.TeacherName,

                    Description = x.Description,
                    Start = x.Start,
                    End = x.End.Value,
                    ThemeColor = x.ThemeColor,
                    IsFullDay = x.IsFullDay
                }).ToList();

                ViewBag.EventList = listEve;

                return View();
            }

        }


        [Authorize(Roles = "assistant,admin")]
        [HttpPost]
        public ActionResult Index(EventViewModel model)
        {
            try
            {
                //List<CalendarForm> listForm = db.CalendarForms.ToList();
                //List<CalendarClassRoom> listClassRoom = db.CalendarClassRooms.ToList();
                //List<CalendarGroup> listGroup = db.CalendarGroups.ToList();
                //List<CalendarSubject> listSubject = db.CalendarSubjects.ToList();
                //List<CalendarTeacher> listTeacher = db.CalendarTeachers.ToList();

                //ViewBag.FormList = new SelectList(listForm, "FormId", "FormName");
                //ViewBag.ClassRoomList = new SelectList(listClassRoom, "ClassRoomId", "ClassRoomName");
                //ViewBag.GroupList = new SelectList(listGroup, "GroupId", "GroupName");
                //ViewBag.SubjectList = new SelectList(listSubject, "SubjectId", "SubjectName");
                //ViewBag.TeacherList = new SelectList(listTeacher, "TeacherId", "TeacherName");

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

                    db.SaveChanges();
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
                    db.SaveChanges();
                }

                return View(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [Authorize(Roles = "admin")]
        public JsonResult DeleteEvent(int EventId)
        {
            bool result = false;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Event ev = db.Events.SingleOrDefault(x => x.EventID == EventId);

                if (ev != null)
                {
                    db.Events.Remove(ev);
                    db.SaveChanges();
                    result = true;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "assistant,admin")]
        public ActionResult ShowEvent(int EventId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<EventViewModel> listEve = db.Events.Where(x => x.EventID == EventId).Select(x => new EventViewModel
                {
                    EventID = x.EventID,

                    FormName = x.CalendarForm.FormName,

                    ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                    ClassRoomPlace = x.CalendarClassRoom.ClassRoomPlace,

                    GroupName = x.CalendarGroup.GroupName,
                    GroupCountPeople = x.CalendarGroup.GroupCountPeople,

                    SubjectName = x.CalendarSubject.SubjectName,
                    SubjectCountLesson = x.CalendarSubject.SubjectCountLesson,

                    TeacherName = x.CalendarTeacher.TeacherName,
                    TeacherEmail = x.CalendarTeacher.TeacherEmail,

                    Description = x.Description,
                    Start = x.Start,
                    End = x.End.Value,
                    ThemeColor = x.ThemeColor,
                    IsFullDay = x.IsFullDay
                }).ToList();

                ViewBag.EventList = listEve;
            }

            return PartialView("Details");
        }


        [Authorize(Roles = "assistant,admin")]
        public ActionResult AddEditEvent(int EventId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
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

                EventViewModel model = new EventViewModel();

                if (EventId > 0)
                {
                    Event ev = db.Events.SingleOrDefault(x => x.EventID == EventId);
                    model.EventID = ev.EventID;
                    model.FormId = ev.CalendarForm.FormId;
                    model.ClassRoomId = ev.CalendarClassRoom.ClassRoomId;
                    model.GroupId = ev.CalendarGroup.GroupId;
                    model.SubjectId = ev.CalendarSubject.SubjectId;
                    model.TeacherId = ev.CalendarTeacher.TeacherId;

                    model.Start = ev.Start;
                    model.End = ev.End;
                    model.IsFullDay = ev.IsFullDay;
                    model.ThemeColor = ev.ThemeColor;
                    model.Description = ev.Description;

                }
                return PartialView("AddEdit", model);
            }

        }


        [Authorize(Roles = "admin")]
        public void ExportAllToExcel()
        {
            List<EventViewModel> evlist = db.Events.Select(x => new EventViewModel
            {
                //EventID = x.EventID,
                FormName = x.CalendarForm.FormName,
                ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                GroupName = x.CalendarGroup.GroupName,
                SubjectName = x.CalendarSubject.SubjectName,
                TeacherName = x.CalendarTeacher.TeacherName,

                Start = x.Start,
                End = x.End
            }).ToList();

            // excel
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Communication";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report All From The Table";

            ws.Cells["A3"].Value = "File Storage Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);


            ws.Cells["A6"].Value = "FormName";
            ws.Cells["B6"].Value = "ClassRoomName";
            ws.Cells["C6"].Value = "GroupName";
            ws.Cells["D6"].Value = "SubjectName";
            ws.Cells["E6"].Value = "TeacherName";
            ws.Cells["F6"].Value = "DateStart";
            ws.Cells["G6"].Value = "DateEnd";

            int rowStart = 7;
            foreach (var item in evlist)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.FormName;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ClassRoomName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.GroupName;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.SubjectName;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.TeacherName;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Start.ToShortDateString() + " " + item.Start.ToShortTimeString();
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.End.Value.ToShortDateString() + " " + item.End.Value.ToShortTimeString();
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "ExportAllToExcel_"+DateTime.Now.ToShortDateString() + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }


        [Authorize(Roles = "admin")]
        public ActionResult SaveSelectedDataToTheExcelFile()
        {
            List<CalendarTeacher> listTeacher = db.CalendarTeachers.ToList();
            ViewBag.TeacherList = new SelectList(listTeacher, "TeacherId", "TeacherName");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult SaveSelectedDataToTheExcelFile(EventViewModel model)
        {
            if (model.TeacherId > 0)
            {
                if(model.Start > model.End)
                {
                    List<CalendarTeacher> listTeacher = db.CalendarTeachers.ToList();
                    ViewBag.TeacherList = new SelectList(listTeacher, "TeacherId", "TeacherName");

                    return View(model);
                }

                if (model.TeacherId > 0 && model.Start != null && model.End != null)
                {
                    var month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Start.Month);

                    List<EventViewModel> evlist = db.Events.Where(x => x.TeacherId == model.TeacherId && x.Start <= model.End && x.Start >= model.Start && x.End <= model.End && x.End >= model.Start).Select(x => new EventViewModel
                    {
                        //EventID = x.EventID,
                        FormName = x.CalendarForm.FormName,
                        ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                        GroupName = x.CalendarGroup.GroupName,
                        SubjectName = x.CalendarSubject.SubjectName,
                        TeacherName = x.CalendarTeacher.TeacherName,

                        Start = x.Start,
                        End = x.End
                    }).ToList();


                    // excel
                    ExcelPackage pck = new ExcelPackage();
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                    ws.Cells["A1"].Value = "Communication: ";
                    ws.Cells["B1"].Value = "Com1";

                    ws.Cells["A2"].Value = "Report: ";
                    ws.Cells["B2"].Value = "A Report For The Teacher";

                    ws.Cells["A3"].Value = "Date: ";
                    ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);


                    ws.Cells["A6"].Value = "TeacherName";
                    ws.Cells["B6"].Value = "FormName";
                    ws.Cells["C6"].Value = "ClassRoomName";
                    ws.Cells["D6"].Value = "GroupName";
                    ws.Cells["E6"].Value = "SubjectName";
                    ws.Cells["F6"].Value = "DateStart";
                    ws.Cells["G6"].Value = "DateEnd";

                    int rowStart = 7;
                    var teacherName = string.Empty;
                    foreach (var item in evlist)
                    {
                        teacherName = item.TeacherName;
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.TeacherName;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.FormName;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.ClassRoomName;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.GroupName;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.SubjectName;
                        ws.Cells[string.Format("F{0}", rowStart)].Value = item.Start.ToShortDateString() + " " + item.Start.ToShortTimeString();
                        ws.Cells[string.Format("G{0}", rowStart)].Value = item.End.Value.ToShortDateString() + " " + item.End.Value.ToShortTimeString();
                        rowStart++;
                    }


                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Report_for_" + teacherName + "_for_"+ month + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();

                    return RedirectToAction("Index");
                }
                else
                {
                    List<EventViewModel> evlist = db.Events.Where(x => x.TeacherId == model.TeacherId).Select(x => new EventViewModel
                    {
                        //EventID = x.EventID,
                        FormName = x.CalendarForm.FormName,
                        ClassRoomName = x.CalendarClassRoom.ClassRoomName,
                        GroupName = x.CalendarGroup.GroupName,
                        SubjectName = x.CalendarSubject.SubjectName,
                        TeacherName = x.CalendarTeacher.TeacherName,

                        Start = x.Start,
                        End = x.End
                    }).ToList();


                    // excel
                    ExcelPackage pck = new ExcelPackage();
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                    ws.Cells["A1"].Value = "Communication: ";
                    ws.Cells["B1"].Value = "Com1";

                    ws.Cells["A2"].Value = "Report: ";
                    ws.Cells["B2"].Value = "A Report For The Teacher";

                    ws.Cells["A3"].Value = "Date: ";
                    ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);


                    ws.Cells["A6"].Value = "TeacherName";
                    ws.Cells["B6"].Value = "FormName";
                    ws.Cells["C6"].Value = "ClassRoomName";
                    ws.Cells["D6"].Value = "GroupName";
                    ws.Cells["E6"].Value = "SubjectName";
                    ws.Cells["F6"].Value = "DateStart";
                    ws.Cells["G6"].Value = "DateEnd";

                    int rowStart = 7;
                    var teacherName = string.Empty;
                    foreach (var item in evlist)
                    {
                        teacherName = item.TeacherName;
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.TeacherName;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.FormName;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.ClassRoomName;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.GroupName;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.SubjectName;
                        ws.Cells[string.Format("F{0}", rowStart)].Value = item.Start.ToShortDateString() + " " + item.Start.ToShortTimeString();
                        ws.Cells[string.Format("G{0}", rowStart)].Value = item.End.Value.ToShortDateString() + " " + item.End.Value.ToShortTimeString();
                        rowStart++;
                    }


                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Report_" + teacherName + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();

                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                List<CalendarTeacher> listTeacher = db.CalendarTeachers.ToList();
                ViewBag.TeacherList = new SelectList(listTeacher, "TeacherId", "TeacherName");

                return View(model);
            }

            

        }




    }
}