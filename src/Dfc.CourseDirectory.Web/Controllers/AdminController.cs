using Dfc.CourseDirectory.Areas.Identity.Data;
using Dfc.CourseDirectory.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dfc.CourseDirectory.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<DfcCourseDirectoryUser> UserManager;
        public AdminController(UserManager<DfcCourseDirectoryUser> userManager)
        {
            this.UserManager = userManager;
        }

        public IEnumerable<DfcCourseDirectoryUser> Users { get; set; }

        public IActionResult Index()
        {
            this.Users = UserManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);
            return View();
        }
        
        public IActionResult ManageUsers()
        {
           return View();
        }
    }
}