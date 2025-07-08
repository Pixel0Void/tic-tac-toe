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

public enum GameState
{
    WaitingForPlayer = 0,
    Active = 1,
    X_Wins = 2,
    O_Wins = 3,
    Draw = 4,
    GameOver = 5
}

[Serializable]
public struct GameStateUpdateDto
{
    public PlayerSymbol[] board;
    public PlayerScoresDto scores;
    public PlayerSymbol currentTurn;
    public GameState gameState;
    public string roomId;
    public int playersCount;

    [Serializable]
    public struct PlayerScoresDto
    {
        public int X;
        public int O;
    }
}

[Serializable]
public struct GameOverDto
{
    public string message;
    public bool gameOver;
    public PlayerSymbol winner;
}

[Serializable]
public struct ErrorDto
{
    public string message;
}

[Serializable]
public struct PlayerDisconnectedDto
{
    public string message;
}


[Serializable]
public struct MakeMoveDto
{
    public int index;
}
