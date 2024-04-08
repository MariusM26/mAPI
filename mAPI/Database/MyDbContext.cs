using mAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace mAPI.Database
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<DCandidate>? DCandidates { get; set; }
    }
}