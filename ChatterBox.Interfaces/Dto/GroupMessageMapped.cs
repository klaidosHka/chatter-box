﻿namespace ChatterBox.Interfaces.Dto;

public class GroupMessageMapped
{
    public DateTime DateSent { get; set; }

    public string GroupId { get; set; }

    public string ImageLink { get; set; }

    public string SenderId { get; set; }

    public string SenderUserName { get; set; }

    public string SignalrId { get; set; }

    public string Text { get; set; }
}