using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ChatUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private Button sendButton;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject messagePrefabOwn;
    [SerializeField] private GameObject messagePrefabOther;
    [SerializeField] private ScrollRect autoScroll;
    [SerializeField] private GameObject replyIndicator;
    [SerializeField] private Button cancelReplyButton;


    private IChatClient _chatClient;
    private ConcurrentQueue<ChatMessage> _messageQueue = new();
    private Dictionary<string, ChatMessage> _messages = new();
    private string _replyToId = null;
    private readonly bool _isOwn = true;

    void Update()
    {
        while (_messageQueue.TryDequeue(out ChatMessage message))
        {
            AddMessageToUI(message, !_isOwn);
        }
    }

    void OnDestroy()
    {
        if (_chatClient is not null)
            _chatClient.OnMessageReceived -= HandleMessageReceived;

        sendButton.onClick.RemoveListener(SendMessage);
        ChatEvents.OnReplyMessageRequested -= HandleReplyMessage;
        cancelReplyButton.onClick.RemoveListener(HandleCancelReply);
    }

    public void Initialize(IChatClient chatClient)
    {
        _chatClient = chatClient;
        _chatClient.OnMessageReceived += HandleMessageReceived;
        sendButton.onClick.AddListener(SendMessage);
        ChatEvents.OnReplyMessageRequested += HandleReplyMessage;
        cancelReplyButton.onClick.AddListener(HandleCancelReply);
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }

    private void HandleMessageReceived(ChatMessage message)
    {
        _messageQueue.Enqueue(message);
    }

    private void AddMessageToUI(ChatMessage message, bool isOwn)
    {
        GameObject prefab = isOwn ? messagePrefabOwn : messagePrefabOther;
        GameObject gameObject = Instantiate(prefab, content);
        MessageItem messageItem = gameObject.GetComponentInChildren<MessageItem>();

        ChatMessage replyToMessage = null;
        if (!string.IsNullOrEmpty(message.ReplyToId))
            _messages.TryGetValue(message.ReplyToId, out replyToMessage);

        messageItem.Setup(message, replyToMessage?.Format(), isOwn);
        _messages[message.Id] = message;
    }

    private void SendMessage()
    {
        if (string.IsNullOrEmpty(messageInput.text))
            return;

        ChatMessage message = _chatClient.SendMessage(messageInput.text, _replyToId);

        _replyToId = null;
        messageInput.text = "";
        replyIndicator.SetActive(false);
        AddMessageToUI(message, _isOwn);
    }

    private void HandleReplyMessage(ChatMessage message)
    {
        replyIndicator.SetActive(true);
        TMP_Text repplyText = replyIndicator.GetComponentInChildren<TMP_Text>();
        repplyText.text = string.Concat("Respondiendo a :", message.Author);
        _replyToId = message.Id;
    }

    private void HandleCancelReply()
    {
        replyIndicator.SetActive(false);
        _replyToId = null;
    }
}