using UnityEngine;

class ChatManager : MonoBehaviour
{
    [SerializeField] private ChatUIController chatUIController;
    [SerializeField] private ConnectionUIController connectionUIController;
    [SerializeField] private TransportProtocol transportProtocol;

    private IChatClient _chatClient;
    private IChatServer _chatServer;

    void Awake()
    {
        ChatEvents.OnChatStartRequested += HandleOnChatstartRequested;
    }

    void OnDestroy()
    {
        ChatEvents.OnChatStartRequested -= HandleOnChatstartRequested;
        _chatServer?.Stop();
        _chatClient?.Disconnect();
    }

    private void HandleOnChatstartRequested(ConnectionConfig config)
    {
        if (config.IsHost())
        {
            _chatServer = CreateChatServer();
            _chatServer.Start(config);
        }


        _chatClient = CreateChatclient();
        _chatClient.Connect(config);

        connectionUIController.gameObject.SetActive(false);
        chatUIController.gameObject.SetActive(true);
        chatUIController.Initialize(_chatClient);
    }

    private IChatServer CreateChatServer()
    {
        switch (transportProtocol)
        {
            case TransportProtocol.TCP:
                return new TCPChatServer();
            case TransportProtocol.UDP:
                return new UDPChatServer();
            default:
                Debug.Log("Invalid Transport Protocol");
                return null;
        }
    }

    private IChatClient CreateChatclient()
    {
        switch (transportProtocol)
        {
            case TransportProtocol.TCP:
                return new TCPChatClient();
            case TransportProtocol.UDP:
                return new UDPChatClient();
            default:
                Debug.Log("Invalid Transport Protocol");
                return null;
        }
    }
}