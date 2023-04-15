using Microsoft.AspNetCore.Identity;

namespace ChatterBox.Interfaces.Entities
{
    public class ChatUser : IdentityUser
    {
        public virtual ICollection<ChatGroupMessage> GroupMessages { get; set; }

        public virtual ICollection<ChatGroup> GroupsOwned { get; set; }

        public virtual ICollection<ChatGroupRegistrar> GroupsRegistrar { get; set; }

        public virtual ICollection<ChatMessage> MessagesReceived { get; set; }

        public virtual ICollection<ChatMessage> MessagesSent { get; set; }
    }
}
