using ChatterBox.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using ChatterBox.Context.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatterBox.Context
{
    public class CbDbContext : IdentityDbContext<ChatUser>
    {
        public DbSet<ChatGroupMessage> ChatGroupMessages { get; set; }

        public DbSet<ChatGroup> ChatGroups { get; set; }

        public DbSet<ChatGroupRegistrar> ChatGroupsRegistrar { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatUser> ChatUsers { get; set; }

        public CbDbContext(DbContextOptions<CbDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ChatGroupConfiguration());
            builder.ApplyConfiguration(new ChatGroupMessageConfiguration());
            builder.ApplyConfiguration(new ChatGroupRegistrarConfiguration());
            builder.ApplyConfiguration(new ChatMessageConfiguration());
        }
    }
}