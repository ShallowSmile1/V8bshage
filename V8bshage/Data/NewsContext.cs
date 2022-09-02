using Microsoft.EntityFrameworkCore;
using V8bshage.Models;

namespace V8bshage.Data
{
    public class NewsContext: DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<News> News { get; set; }
    }
}
