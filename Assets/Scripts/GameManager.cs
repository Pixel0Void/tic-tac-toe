using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerSymbol m_MyPlayerSymbol = PlayerSymbol.None;
    private string m_CurrentRoomId = "";
    private PlayerSymbol m_CurrentTurnSymbol = PlayerSymbol.None;
    private PlayerSymbol[] m_BoardState = new PlayerSymbol[9];
    private Dictionary<PlayerSymbol, int> m_CurrentScores = new Dictionary<PlayerSymbol, int>();

    void Start()
    {
        if (NetworkManager.Instance != null && NetworkManager.Instance.Socket != null)
        {
            NetworkManager.Instance.Socket.OnUnityThread("playerAssigned", OnPlayerAssigned);
            NetworkManager.Instance.Socket.OnUnityThread("roomJoined", OnRoomJoined);
            NetworkManager.Instance.Socket.OnUnityThread("gameReady", OnGameReady);
            NetworkManager.Instance.Socket.OnUnityThread("gameStateUpdate", OnGameStateUpdate);
            NetworkManager.Instance.Socket.OnUnityThread("resetGame", OnResetGame);
            NetworkManager.Instance.Socket.OnUnityThread("gameOver", OnGameOver);
            NetworkManager.Instance.Socket.OnUnityThread("error", OnServerError);
            NetworkManager.Instance.Socket.OnUnityThread("playerDisconnected", OnPlayerDisconnectedInternal);
        }
        else
        {
            Debug.LogError("NetworkManager or socket not found.");
        }
    }

    void OnPlayerAssigned(SocketIOResponse response)
    {
        var data = response.GetValue<PlayerAssignedDto>();
        m_MyPlayerSymbol = data.symbol;
        Debug.Log($"<color=yellow> Your sign: {m_MyPlayerSymbol}</color>");
    }

    void OnRoomJoined(SocketIOResponse response)
    {
        var data = response.GetValue<RoomJoinedDto>();
        m_CurrentRoomId = data.roomId;
        Debug.Log($"<color=green>Successfully joined to room '{m_CurrentRoomId}'.</color>");
    }

    void OnGameReady(SocketIOResponse response)
    {
        var data = response.GetValue<string>();
        Debug.Log($"<color=green>{data}</color>");
    }

    void OnGameStateUpdate(SocketIOResponse response)
    {
        var data = response.GetValue<GameStateUpdateDto>();
        m_BoardState = data.board;

        m_CurrentScores[PlayerSymbol.X] = data.scores.X;
        m_CurrentScores[PlayerSymbol.O] = data.scores.O;

        m_CurrentTurnSymbol = data.currentTurn;
    }

    void OnResetGame(SocketIOResponse response)
    {
        Debug.Log("<color=green>Game is reseting...</color>");
    }

    void OnGameOver(SocketIOResponse response)
    {
        var data = response.GetValue<GameOverDto>();
        string result = "";
        if (data.gameOver || data.winner != PlayerSymbol.None)
        {
            result = data.message + $"Winner: {data.winner}";
        }
        else
        {
            result = "Draw!";
        }
        Debug.Log($"<color=purple>{result}</color>");
    }

    void OnServerError(SocketIOResponse response)
    {
        var data = response.GetValue<ErrorDto>();
        Debug.LogError($"<color=red>Server error: {data.message}</color>");
    }

    void OnPlayerDisconnectedInternal(SocketIOResponse response)
    {
        var data = response.GetValue<PlayerDisconnectedDto>();
        Debug.Log($"<color=purple>{data.message}</color>");
    }
}
