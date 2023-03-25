using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ChatterBox.Context
{
    public class CbDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public CbDbContext(DbContextOptions<CbDbContext> options)
            : base(options) { }
    }
}