using System;

public enum PlayerSymbol
{
    None = 0,
    X = 1,
    O = 2
}

[Serializable]
public struct PlayerAssignedDto
{
    public PlayerSymbol symbol;
}

[Serializable]
public struct RoomJoinedDto
{
    public string roomId;
}
