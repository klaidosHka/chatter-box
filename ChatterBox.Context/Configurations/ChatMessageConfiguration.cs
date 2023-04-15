using ChatterBox.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatterBox.Context.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable("ChatMessages");

            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.DateSent)
                .IsRequired();

            builder
                .Property(m => m.Id)
                .IsRequired();

            builder
                .Property(m => m.ImageLink)
                .IsRequired(false);

            builder
                .Property(m => m.ReceiverId)
                .IsRequired();

            builder
                .Property(m => m.SenderId)
                .IsRequired();

            builder
                .Property(m => m.Text)
                .IsRequired(false);

            builder
                .HasOne(m => m.Receiver)
                .WithMany(r => r.MessagesReceived)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(m => m.Sender)
                .WithMany(s => s.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("message_check_any_content", $"{nameof(ChatMessage.Text)} IS NOT NULL OR {nameof(ChatMessage.ImageLink)} IS NOT NULL");
        }
    }
}
