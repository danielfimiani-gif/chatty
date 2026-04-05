using System;

public static class ChatEvents
{
    public static Action<ConnectionConfig> OnChatStartRequested;
    public static Action<ChatMessage> OnReplyMessageRequested;
}