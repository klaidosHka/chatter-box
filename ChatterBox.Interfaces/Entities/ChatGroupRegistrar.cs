namespace ChatterBox.Interfaces.Entities
{
    public class ChatGroupRegistrar
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public virtual ChatUser User { get; set; }

        public string GroupId { get; set; }

        public virtual ChatGroup Group { get; set; }
    }
}
