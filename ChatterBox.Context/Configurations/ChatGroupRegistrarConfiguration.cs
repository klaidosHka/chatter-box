using ChatterBox.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatterBox.Context.Configurations
{
    public class ChatGroupRegistrarConfiguration : IEntityTypeConfiguration<ChatGroupRegistrar>
    {
        public void Configure(EntityTypeBuilder<ChatGroupRegistrar> builder)
        {
            builder.ToTable("ChatGroupsRegistrar");

            builder.HasKey(r => r.Id);

            builder
                .Property(r => r.GroupId)
                .IsRequired();

            builder
                .Property(r => r.Id)
                .IsRequired();

            builder
                .Property(r => r.UserId)
                .IsRequired();

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.GroupsRegistrar)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(r => r.Group)
                .WithMany(g => g.GroupsRegistrar)
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
