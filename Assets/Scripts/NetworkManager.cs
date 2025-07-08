using System;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    public SocketIOUnity Socket;
    public string connectionUri = "http://127.0.0.1:3000";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSocketIO();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSocketIO()
    {
        var uri = new Uri(connectionUri);
        var options = new SocketIOOptions()
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        };
        Socket = new SocketIOUnity(uri, options);
        Socket.JsonSerializer = new NewtonsoftJsonSerializer();

        Socket.OnConnected += (sender, e) =>
        {
            Debug.Log("<color=green>Socket.IO Connected!</color>");
        };
        Socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("<color=red>Socket.IO Disconnected!</color>");
        };
        Socket.OnPing += (sender, e) =>
        {
            Debug.Log("Ping");
        };
        Socket.OnPong += (sender, e) =>
        {
            Debug.Log("Pong: " + e.TotalMilliseconds);
        };
        Socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.LogWarning($"<color=orange>Socket.IO Reconnection Attempt: {e} </color>");
        };
        Socket.OnReconnectFailed += (sender, e) =>
        {
            Debug.LogError("<color=red>Scoket.IO Reconnect Failed </color>");
        };
        Socket.OnReconnectError += (sender, e) =>
        {
            Debug.Log($"<color=red> Socket.IO Reconnect Error: {e} </color>");
        };
        Socket.OnError += (sender, e) =>
        {
            Debug.LogError($"<color=red>Socket.IO Error: {e} </color>");
        };

        Debug.Log($"Attempting to connect to: {connectionUri}");
        Socket.Connect();
    }

    void OnApplicationQuit()
    {
        if (Socket != null && Socket.Connected)
        {
            Socket.Disconnect();
            Debug.Log("[SocketIO] Disconnected on Application Quit.");
        }
        Socket?.Dispose();
    }
}
