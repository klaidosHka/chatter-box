﻿using System.Security.Claims;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Dto;

public class UserMapped
{
    public string AvatarLink { get; set; }

    public bool Online { get; set; }

    public ClaimsPrincipal Principal { get; set; }

    public ChatUser User { get; set; }
}