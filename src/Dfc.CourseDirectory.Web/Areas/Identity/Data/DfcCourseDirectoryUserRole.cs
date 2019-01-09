using Dfc.CourseDirectory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfc.CourseDirectory.Web.Areas.Identity.Data
{
    public class DfcCourseDirectoryUserRole : IdentityUserRole<string>
    {
        public virtual DfcCourseDirectoryUser User { get; set; }
        public virtual DfcCourseDirectoryUser Role { get; set; }
    }
}
