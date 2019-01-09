using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfc.CourseDirectory.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Dfc.CourseDirectory.Web.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the DfcCourseDirectory class
    public class DfcCourseDirectoryUser : IdentityUser
    {
        public ICollection<DfcCourseDirectoryUserRole> UserRoles { get; set; }
    }
}
