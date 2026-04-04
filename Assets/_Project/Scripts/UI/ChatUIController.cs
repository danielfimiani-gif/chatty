using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ChatUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private Button sendButton;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private ScrollRect autoScroll;

    private ChatClient _chatClient;
    private ConcurrentQueue<string> _messageQueue = new();

    void Update()
    {
        while (_messageQueue.TryDequeue(out string message))
        {
            AddMessageToUI(message);
        }
    }

    void OnDestroy()
    {
        if (_chatClient is not null)
            _chatClient.OnMessageReceived -= HandleMessageReceived;

        sendButton.onClick.RemoveListener(SendMessage);
    }

    public void Initialize(ChatClient chatClient)
    {
        _chatClient = chatClient;
        _chatClient.OnMessageReceived += HandleMessageReceived;
        sendButton.onClick.AddListener(SendMessage);
    }

    private void HandleMessageReceived(string message)
    {
        _messageQueue.Enqueue(message);
    }

    private void AddMessageToUI(string message)
    {
        GameObject messageItem = Instantiate(messagePrefab, content);
        TMP_Text text = messageItem.GetComponent<TMP_Text>();
        text.text = message;
    }

    private void SendMessage()
    {
        string message = messageInput.text;

        if (string.IsNullOrEmpty(message))
            return;

        _chatClient.SendMessage(message);

        messageInput.text = "";
        AddMessageToUI(message);
    }
}