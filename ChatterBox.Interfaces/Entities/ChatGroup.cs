namespace ChatterBox.Interfaces.Entities
{
    public class ChatGroup
    {
        public string Id { get; set; }

        public virtual ICollection<ChatGroupMessage> Messages { get; set; }

        public virtual ICollection<ChatGroupRegistrar> GroupsRegistrar { get; set; }

        public string Name { get; set; }

        public virtual ChatUser Owner { get; set; }

        public string OwnerId { get; set; }
    }
}
