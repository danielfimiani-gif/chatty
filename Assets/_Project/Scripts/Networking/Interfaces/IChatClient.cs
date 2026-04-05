using System;

public interface IChatClient
{
    event Action<ChatMessage> OnMessageReceived;
    void Connect(ConnectionConfig config);
    ChatMessage SendMessage(string content, string replyToId);
    void Disconnect();
}