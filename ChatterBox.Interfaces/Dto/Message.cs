namespace ChatterBox.Interfaces.Dto;

public class Message
{
    public DateTime DateSent { get; set; }

    public string Text { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }
}