using SocketIOClient;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerSymbol m_MyPlayerSymbol = PlayerSymbol.None;
    private string m_CurrentRoomId = "";

    void Start()
    {
        if (NetworkManager.Instance != null && NetworkManager.Instance.Socket != null)
        {
            NetworkManager.Instance.Socket.OnUnityThread("playerAssigned", OnPlayerAssigned);
            NetworkManager.Instance.Socket.OnUnityThread("roomJoined", OnRoomJoined);
            NetworkManager.Instance.Socket.OnUnityThread("gameReady", OnGameReady);
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
}
