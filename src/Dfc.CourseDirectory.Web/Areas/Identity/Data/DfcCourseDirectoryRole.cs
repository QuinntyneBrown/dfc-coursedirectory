using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfc.CourseDirectory.Web.Areas.Identity.Data
{
    public class DfcCourseDirectoryRole : IdentityRole
    {
        public ICollection<DfcCourseDirectoryUserRole> UserRoles { get; set; }
    }
}
