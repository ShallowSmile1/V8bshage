using System;
using Microsoft.EntityFrameworkCore;
using V8bshage.Models;

namespace V8bshage.Data
{
    public class AdvertisementContext : DbContext
    {
        public AdvertisementContext(DbContextOptions<AdvertisementContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Advertisement> Advertisements { get; set; }
    }
}
