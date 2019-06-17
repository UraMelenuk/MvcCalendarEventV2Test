using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCalendarEventV2Test.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    // adding classes
    public class Event                                          // Подія
    {
        [Key]
        public int EventID { get; set; }                        // подія id

        public int? FormId { get; set; }                        // calendar form id
        public virtual CalendarForm CalendarForm { get; set; }

        public int? ClassRoomId { get; set; }                   // calendar classroom id
        public virtual CalendarClassRoom CalendarClassRoom { get; set; }

        public int? GroupId { get; set; }                       // calendar group id
        public virtual CalendarGroup CalendarGroup { get; set; }

        public int? SubjectId { get; set; }                     // calendar subject id
        public virtual CalendarSubject CalendarSubject { get; set; }

        public int? TeacherId { get; set; }                     // calendar teacher id
        public virtual CalendarTeacher CalendarTeacher { get; set; }

        public string Description { get; set; }                 // опис
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm A}")]
        public System.DateTime Start { get; set; }              // дата початок
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm A}")]
        public Nullable<System.DateTime> End { get; set; }      // дата кінець
        public string ThemeColor { get; set; }                  // колір
        public bool IsFullDay { get; set; }                     // повний день

    }

    public class EventViewModel
    {
        [Required(ErrorMessage ="Enter Form")]
        public int FormId { get; set; }
        public string FormName { get; set; }

        [Required(ErrorMessage = "Enter ClassRoom")]
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public int ClassRoomPlace { get; set; }

        [Required(ErrorMessage = "Enter Group")]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupCountPeople { get; set; }

        [Required(ErrorMessage = "Enter Subject")]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int SubjectCountLesson { get; set; }

        [Required(ErrorMessage = "Enter Teacher")]
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherMessage { get; set; }
        [NotMapped]
        public HttpPostedFileBase UploadFile { get; set; }

        public int EventID { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm A}")]
        public System.DateTime Start { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm A}")]
        public Nullable<System.DateTime> End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }

    }
    //
    public class Users_in_Role_ViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    //
    public class User : IdentityUser          // delete users account
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    //
    public class CalendarForm                                   // Форма навчання
    {
        [Key]
        public int FormId { get; set; }                         // форма id
        [Required(ErrorMessage = "Field is not filled : FormName")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The length of the string must be between 3 and 30 characters")]
        public string FormName { get; set; }                    // назва форми

        public ICollection<Event> CalendarForms { get; set; }
        public CalendarForm()
        {
            CalendarForms = new List<Event>();
        }
    }
    //
    public class CalendarClassRoom                              // Аудиторія
    {
        [Key]
        public int ClassRoomId { get; set; }                    // аудиторія id
        [Required(ErrorMessage = "Field is not filled : ClassRoom")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The length of the string must be between 1 and 30 characters")]
        public string ClassRoomName { get; set; }               // назва аудиторії
        [Required(ErrorMessage = "Field is not filled : ClassRoomPlace")]
        public int ClassRoomPlace { get; set; }                 // вміст

        public ICollection<Event> CalendarClassRooms { get; set; }
        public CalendarClassRoom()
        {
            CalendarClassRooms = new List<Event>();
        }

    }
    //
    public class CalendarGroup                                  // Група
    {
        [Key]
        public int GroupId { get; set; }                        // група id
        [Required(ErrorMessage = "Field is not filled : GroupName")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The length of the string must be between 1 and 30 characters")]
        public string GroupName { get; set; }                   // назва групи
        [Required(ErrorMessage = "Field is not filled : GroupCountPeople")]
        public int GroupCountPeople { get; set; }               // кількість людей

        public ICollection<Event> CalendarGroups { get; set; }
        public CalendarGroup()
        {
            CalendarGroups = new List<Event>();
        }

    }
    //
    public class CalendarSubject                                // Предмет
    {
        [Key]
        public int SubjectId { get; set; }                      // предмет id
        [Required(ErrorMessage = "Field is not filled : SubjectName")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The length of the string must be between 1 and 30 characters")]
        public string SubjectName { get; set; }                 // назва предмета
        [Required(ErrorMessage = "Field is not filled : SubjectCountLesson")]
        public int SubjectCountLesson { get; set; }             // кількість уроків

        public ICollection<Event> CalendarSubjects { get; set; }
        public CalendarSubject()
        {
            CalendarSubjects = new List<Event>();
        }

    }
    //
    public class CalendarTeacher                                // Викладач
    {
        [Key]
        public int TeacherId { get; set; }                      // викладач id
        [Required(ErrorMessage = "Field is not filled : TeacherName")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The length of the string must be between 1 and 30 characters")]
        public string TeacherName { get; set; }                 // назва викладача
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid mail address")]
        public string TeacherEmail { get; set; }                // емейл викладача

        public string TeacherMessage { get; set; }              // повідомлення
        [NotMapped]
        public HttpPostedFileBase UploadFile { get; set; }      // файл для отправки

        public ICollection<Event> CalendarTeachers { get; set; }
        public CalendarTeacher()
        {
            CalendarTeachers = new List<Event>();
        }

    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // adding classes
        public DbSet<Event> Events { get; set; }
        public DbSet<CalendarForm> CalendarForms { get; set; }
        public DbSet<CalendarClassRoom> CalendarClassRooms { get; set; }
        public DbSet<CalendarGroup> CalendarGroups { get; set; }
        public DbSet<CalendarSubject> CalendarSubjects { get; set; }
        public DbSet<CalendarTeacher> CalendarTeachers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        object placeHolderVariable;
    }


}