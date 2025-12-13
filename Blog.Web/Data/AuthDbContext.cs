using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Suppress all model warnings
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));  // This ignores model validation issues in general

            // other configuration
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);


            //Seed Roles(User, Admin. SuperAdmin)

            var adminRoleId = "308017c8-1fb4-4b62-9670-e0711347a1ae";
            var superAdminRoleId = "5dfea4ae-e526-4730-bf0c-2d615be0ea26";
            var userRoleId = "8c12d83c-51c1-4839-a09a-05776724d256";

            



            var roles = new List<IdentityRole>
            {
                 new IdentityRole { 
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId

                 },
                 new IdentityRole { 
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId

                 },
                 new IdentityRole { 
                     Name = "User",
                     NormalizedName = "User",
                     Id = userRoleId,
                     ConcurrencyStamp = userRoleId
                 }
            };


            modelBuilder.Entity<IdentityRole>().HasData(roles);


            //SuperAdmin
            var superAdminId = "8e65bc18-3d8e-4efa-b7cb-be107d8c2bd7";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@crud.com",
                Email = "superadmin@crud.com",
                NormalizedEmail = "superadmin@crud.com".ToUpper(),
                NormalizedUserName = "superadmin@crud.com".ToUpper(),
                Id = superAdminId,
                ConcurrencyStamp = superAdminId
            };


            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAdmin@123");

            modelBuilder.Entity<IdentityUser>().HasData(superAdminUser);

            //add all role to superadminuser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string> {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string> {
                    RoleId = userRoleId,
                    UserId = superAdminId
                },
            };


            modelBuilder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


            
        }
    }
}
