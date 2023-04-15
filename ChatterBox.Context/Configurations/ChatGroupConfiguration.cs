using ChatterBox.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatterBox.Context.Configurations
{
    public class ChatGroupConfiguration : IEntityTypeConfiguration<ChatGroup>
    {
        public void Configure(EntityTypeBuilder<ChatGroup> builder)
        {
            builder.ToTable("ChatGroups");

            builder.HasKey(g => g.Id);

            builder
                .Property(g => g.Name)
                .IsRequired();

            builder
                .Property(g => g.Id)
                .IsRequired();

            builder
                .Property(g => g.OwnerId)
                .IsRequired();

            builder
                .HasOne(g => g.Owner)
                .WithMany(o => o.GroupsOwned)
                .HasForeignKey(g => g.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
