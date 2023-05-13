namespace ChatterBox.Interfaces.Dto;

public class SendMessageResponse
{
    public DateTime DateSent { get; set; }

    public string ReceiverId { get; set; }

    public string SenderId { get; set; }

    public string SenderUserName { get; set; }

    public string SignalrId { get; set; }

    public string Text { get; set; }
}