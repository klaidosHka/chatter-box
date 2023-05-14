namespace ChatterBox.Interfaces.Dto;

public class SendGroupMessageRequest
{
    public DateTime DateSent { get; set; }

    public string FileBytes { get; set; }

    public string FileName { get; set; }

    public string GroupId { get; set; }

    public string SenderId { get; set; }

    public string SignalrId { get; set; }

    public string Text { get; set; }
}