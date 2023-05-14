namespace ChatterBox.Interfaces.Dto;

public class CreateGroupResponse
{
    public string GroupId { get; set; }

    public string MessageError { get; set; }

    public bool Success { get; set; }
}