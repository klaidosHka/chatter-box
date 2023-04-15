namespace ChatterBox.Interfaces.Entities
{
    public class ChatGroupMessage
    {
        public DateTime DateSent { get; set; }

        public virtual ChatGroup Group { get; set; }

        public string GroupId { get; set; }

        public string Id { get; set; }

        public string ImageLink { get; set; }

        public virtual ChatUser Sender { get; set; }

        public string SenderId { get; set; }

        public string Text { get; set; }
    }
}
