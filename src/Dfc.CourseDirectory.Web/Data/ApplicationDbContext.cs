using Dfc.CourseDirectory.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dfc.CourseDirectory.Data
{
    public class ApplicationDbContext : IdentityDbContext<DfcCourseDirectoryUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
