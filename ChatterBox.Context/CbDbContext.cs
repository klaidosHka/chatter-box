using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Context
{
    public class CbDbContext : IdentityDbContext
    {
        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

        public CbDbContext(DbContextOptions<CbDbContext> options) : base(options)
        {

        }
    }
}