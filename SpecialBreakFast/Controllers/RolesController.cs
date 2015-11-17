using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpecialBreakFast.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SpecialBreakFast.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }

        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ManageUserRoles()
        {
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>

new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            var userList = context.Users.OrderBy(r => r.Email).ToList().Select(rr => new SelectListItem { Value = rr.Email.ToString(), Text = rr.Email }).ToList();
            ViewBag.userNames = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(ManageRoleViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;

                var userList = context.Users.OrderBy(r => r.Email).ToList().Select(rr=> new SelectListItem { Value = rr.Email.ToString(), Text = rr.Email }).ToList();
                ViewBag.userNames = userList;
            }

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }



    }
}