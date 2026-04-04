using UnityEngine;

class ChatManager : MonoBehaviour
{
    [SerializeField] private ChatUIController chatUIController;
    [SerializeField] private ConnectionUIController connectionUIController;

    private ChatClient _chatClient;
    private ChatServer _chatServer;

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
            _chatServer = new ChatServer();
            _chatServer.Start(config.Port);
        }


        _chatClient = new ChatClient();
        _chatClient.Connect(config.Ip, config.Port);

        connectionUIController.gameObject.SetActive(false);
        chatUIController.gameObject.SetActive(true);
        chatUIController.Initialize(_chatClient);
    }
}