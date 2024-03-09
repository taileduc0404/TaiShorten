using Microsoft.EntityFrameworkCore;
using TaiShorten.Models;

namespace TaiShorten.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ShortUrl>? shortUrls { get; set; }
    }
}
