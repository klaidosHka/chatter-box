using ChatterBox.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatterBox.Context.Configurations
{
    public class ChatGroupMessageConfiguration : IEntityTypeConfiguration<ChatGroupMessage>
    {
        public void Configure(EntityTypeBuilder<ChatGroupMessage> builder)
        {
            builder.ToTable("ChatGroupMessages");

            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.DateSent)
                .IsRequired();

            builder
                .Property(m => m.GroupId)
                .IsRequired();

            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.ImageLink)
                .IsRequired(false);

            builder
                .Property(m => m.SenderId)
                .IsRequired();

            builder
                .Property(m => m.Text)
                .IsRequired(false);

            builder
                .HasOne(m => m.Group)
                .WithMany(g => g.Messages)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(m => m.Sender)
                .WithMany(s => s.GroupMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("group_message_check_any_content", $"{nameof(ChatGroupMessage.Text)} IS NOT NULL OR {nameof(ChatGroupMessage.ImageLink)} IS NOT NULL");
        }
    }
}
