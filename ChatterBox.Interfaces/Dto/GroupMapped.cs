namespace ChatterBox.Interfaces.Dto;

public class GroupMapped
{
    public string Id { get; set; }

    public bool Joined { get; set; }

    public string Name { get; set; }

    public string OwnerId { get; set; }
}