using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcCalendarEventV2Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var usersWithRoles = (from user in db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in db.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new User()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(" , ", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }

        //
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser user = UserManager.FindById(id);

            if (user != null)
            {
                IdentityResult Result = UserManager.Delete(user);
                if(Result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                    
                }
            }

            return View();
        }




    }
}