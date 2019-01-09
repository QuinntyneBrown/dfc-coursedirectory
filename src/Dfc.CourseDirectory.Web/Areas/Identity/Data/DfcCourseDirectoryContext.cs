using Dfc.CourseDirectory.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dfc.CourseDirectory.Web.Areas.Identity.Data
{
    public class DfcCourseDirectoryContext 
        : IdentityDbContext<DfcCourseDirectoryUser, DfcCourseDirectoryRole, string, IdentityUserClaim<string>, 
            DfcCourseDirectoryUserRole, IdentityUserLogin<string>, 
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DfcCourseDirectoryContext(DbContextOptions<DfcCourseDirectoryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DfcCourseDirectoryUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
               
            });
            
        }
    }
}
