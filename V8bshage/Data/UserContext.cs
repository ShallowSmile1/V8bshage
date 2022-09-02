using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using V8bshage.Models;


namespace V8bshage.Data
{
    public class UserContext: IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
