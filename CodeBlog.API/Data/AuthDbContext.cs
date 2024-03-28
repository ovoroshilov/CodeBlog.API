using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "849c2448-c1c6-4617-9b77-f68408a96d09";
        var writerRoleId = "f432abbc-1aee-4bc3-884e-f58f1939b24d";

        var roles = new List<IdentityRole>()
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp= readerRoleId
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp= writerRoleId
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);

        var adminId = "4cb2a21b-7e8c-482c-9c5d-3957df0c63b2";

        var admin = new IdentityUser()
        {
            Id = adminId,
            UserName = "Oleg123",
            Email = "olegvoroshilov123@gmail.com",
            NormalizedEmail = "olegvoroshilov123@gmail.com".ToUpper(),
            NormalizedUserName = "Oleg123".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin123");


        builder.Entity<IdentityUser>().HasData(admin);

        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = adminId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminId,
                RoleId = writerRoleId
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}
