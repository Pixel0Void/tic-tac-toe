using SocketIOClient;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerSymbol m_MyPlayerSymbol = PlayerSymbol.None;

    void Start()
    {
        if (NetworkManager.Instance != null && NetworkManager.Instance.Socket != null)
        {
            NetworkManager.Instance.Socket.OnUnityThread("playerAssigned", OnPlayerAssigned);
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
}
