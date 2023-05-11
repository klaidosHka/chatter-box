namespace ChatterBox.Interfaces.Dto;

public class SendMessageResponseRequest
{
    public DateTime DateSent { get; set; }

    public string Id { get; set; }

    public string Text { get; set; }

    public string UserName { get; set; }
}