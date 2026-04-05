using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

class TCPChatClient : IChatClient
{
    public event Action<ChatMessage> OnMessageReceived;

    private TcpClient _client;
    private Thread _thread;
    private string _userName;


    public void Connect(ConnectionConfig config)
    {
        _client = new TcpClient();
        _client.Connect(config.Ip, config.Port);
        _thread = new Thread(ListenForMessages)
        {
            IsBackground = true,
        };
        _thread.Start();
        _userName = config.UserName;
    }

    public void ListenForMessages()
    {
        try
        {
            NetworkStream stream = _client.GetStream();
            while (true)
            {
                byte[] lengthBuffer = new byte[4];
                int totalRead = 0;
                while (totalRead < 4)
                {
                    int bytesRead = stream.Read(lengthBuffer, totalRead, 4 - totalRead);
                    if (bytesRead == 0)
                        return;
                    totalRead += bytesRead;
                }

                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                byte[] messageBuffer = new byte[messageLength];
                totalRead = 0;
                while (totalRead < messageLength)
                {
                    int bytesRead = stream.Read(messageBuffer, totalRead, messageLength - totalRead);
                    if (bytesRead == 0)
                        return;
                    totalRead += bytesRead;
                }

                string jsonMessage = Encoding.UTF8.GetString(messageBuffer);
                ChatMessage message = JsonUtility.FromJson<ChatMessage>(jsonMessage);
                OnMessageReceived?.Invoke(message);
            }
        }
        catch (System.Exception)
        {
            _client?.Close();
        }

    }

    public ChatMessage SendMessage(string content, string replyToId)
    {
        ChatMessage message = new(_userName, content, replyToId);
        string jsonMessage = JsonUtility.ToJson(message);
        byte[] bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);
        byte[] lengthBytes = BitConverter.GetBytes(bytesMessage.Length);

        NetworkStream stream = _client.GetStream();
        stream.Write(lengthBytes, 0, lengthBytes.Length);
        stream.Write(bytesMessage, 0, bytesMessage.Length);

        return message;
    }

    public void Disconnect()
    {
        _client?.Close();
    }
}