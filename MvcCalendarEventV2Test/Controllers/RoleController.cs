using MvcCalendarEventV2Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MvcCalendarEventV2Test.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Role
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        // GET: RoleAddToUser
        [Authorize(Roles = "admin")]
        public ActionResult RoleAddToUser()
        {
            // prepopulat roles for the view dropdown
            var listuser = db.Users.OrderBy(u => u.UserName).Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Users = listuser;
            ViewBag.Roles = list;

            return View();
        }

        // POST: RoleAddToUser
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //var account = new AccountController();
                //account.UserManager.AddToRole(user.Id, RoleName);

                var account = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var idResult = account.AddToRole(user.Id, RoleName);

                ViewBag.ResultMessage = "Role created successfully !";

                // prepopulat roles for the view dropdown
                var listuser = db.Users.Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

                var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                ViewBag.Users = listuser;
                ViewBag.Roles = list;

                return Redirect("/Admin/Index");
            }
            return View("RoleAddToUser");
        }

        // GET: GetRoles
        [Authorize(Roles = "admin")]
        public ActionResult GetRoles()
        {
            // prepopulat roles for the view dropdown
            var listuser = db.Users.OrderBy(u => u.UserName).Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Users = listuser;
            ViewBag.Roles = list;

            return View();
        }

        // POST: GetRoles
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                //var account = new AccountController();
                var account = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                ViewBag.RolesForThisUser = account.GetRoles(user.Id); //account.UserManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var listuser = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
                var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Users = listuser;
                ViewBag.Roles = list;
            }

            return View("GetRoles");
        }

        // GET: DeleteRoleForUser
        [Authorize(Roles = "admin")]
        public ActionResult DeleteRoleForUser()
        {
            var listuser = db.Users.OrderBy(u => u.UserName).Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Users = listuser;
            ViewBag.Roles = list;

            return View();
        }

        // POST: DeleteRoleForUser
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            if (ModelState.IsValid)
            {
                //var account = new AccountController();
                var account = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (account.IsInRole(user.Id, RoleName))
                {
                    account.RemoveFromRole(user.Id, RoleName);
                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }
                // prepopulat roles for the view dropdown
                var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;

                return Redirect("/Admin/Index");
            }
            return View("DeleteRoleForUser");
        }



    }
}