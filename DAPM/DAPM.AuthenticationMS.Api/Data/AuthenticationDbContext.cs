using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAPM.AuthenticationMS.Api.Data;

public class AuthenticationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
    : base(options)
        {
            InitializeDatabase();
        }

    public void InitializeDatabase()
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.EnsureDeleted();
            Database.Migrate();

            SaveChanges();
        }
    }
}
