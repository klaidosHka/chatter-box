namespace ChatterBox.Interfaces.Entities
{
    public class ChatMessage
    {
        public DateTime DateSent { get; set; }

        public string Id { get; set; }

        public string ImageLink { get; set; }

        public virtual ChatUser Receiver { get; set; }

        public string ReceiverId { get; set; }

        public virtual ChatUser Sender { get; set; }

        public string SenderId { get; set; }

        public string Text { get; set; }
    }
}
