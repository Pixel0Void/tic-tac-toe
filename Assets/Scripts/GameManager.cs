using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public UIManager UIManager;

    private PlayerSymbol m_MyPlayerSymbol = PlayerSymbol.None;
    private string m_CurrentRoomId = "";
    private PlayerSymbol m_CurrentTurnSymbol = PlayerSymbol.None;
    private PlayerSymbol[] m_BoardState = new PlayerSymbol[9];
    private Dictionary<PlayerSymbol, int> m_CurrentScores = new Dictionary<PlayerSymbol, int>();

    void Awake()
    {
        m_CurrentScores[PlayerSymbol.X] = 0;
        m_CurrentScores[PlayerSymbol.O] = 0;

        UIManager.SetCellsInteractable(false);

        UIManager.UpdateScoreBoard(m_MyPlayerSymbol, m_CurrentScores[PlayerSymbol.X], m_CurrentScores[PlayerSymbol.O]);
        UIManager.AddListenerToCells(OnCellClicked);
    }

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

    public void OnCellClicked(int inx)
    {
        if (m_MyPlayerSymbol == m_CurrentTurnSymbol && m_BoardState[inx] == PlayerSymbol.None)
        {
            Debug.Log($"<color=orange>Player {m_MyPlayerSymbol} clicked on cell {inx}. Emitting 'makeMove' ... </color>");
            NetworkManager.Instance.Socket.Emit("makeMove", new MakeMoveDto { index = inx });
            UIManager.UpdateCell(inx, m_MyPlayerSymbol);
            UIManager.SetCellInteractable(inx, false);
        }
        else
        {
            Debug.LogWarning("It's not your turn or this cell is occupied");
        }
    }

    void OnPlayerAssigned(SocketIOResponse response)
    {
        var data = response.GetValue<PlayerAssignedDto>();
        m_MyPlayerSymbol = data.symbol;
        UIManager.SetSigns(m_MyPlayerSymbol);
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
        UIManager.GoToGame();
    }

    void OnGameStateUpdate(SocketIOResponse response)
    {
        var data = response.GetValue<GameStateUpdateDto>();
        m_BoardState = data.board;
        UIManager.UpdateCells(m_BoardState);

        m_CurrentScores[PlayerSymbol.X] = data.scores.X;
        m_CurrentScores[PlayerSymbol.O] = data.scores.O;
        UIManager.UpdateScoreBoard(m_MyPlayerSymbol, m_CurrentScores[PlayerSymbol.X], m_CurrentScores[PlayerSymbol.O]);

        m_CurrentTurnSymbol = data.currentTurn;

        bool gameIsActive = data.gameState == GameState.Active;
        bool isMyTurn = m_MyPlayerSymbol == m_CurrentTurnSymbol;

        UIManager.UpdateTurn(isMyTurn);

        for (int i = 0; i < m_BoardState.Length; i++)
        {
            bool cellIsEmpty = m_BoardState[i] == PlayerSymbol.None;
            UIManager.SetCellInteractable(i, gameIsActive && isMyTurn && cellIsEmpty);
        }

        Debug.Log($"<color=blue>Turn: {m_CurrentTurnSymbol}. The full game state updated: state: {data.gameState}, room: {data.roomId}</color>");
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
        UIManager.InitialEndGamePanel(data.gameOver, result);
        UIManager.SetCellsInteractable(false);
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
        UIManager.InitialEndGamePanel(true, data.message);
        UIManager.SetCellsInteractable(false);
    }
}
