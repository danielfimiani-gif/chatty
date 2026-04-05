using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

class UDPChatClient : IChatClient
{
    public event Action<ChatMessage> OnMessageReceived;

    private UdpClient _client;
    private Thread _thread;
    private string _userName;

    public void Connect(ConnectionConfig config)
    {
        _client = new UdpClient();
        _client.Connect(config.Ip, config.Port);
        _thread = new Thread(ListenForMessages)
        {
            IsBackground = true,
        };
        _thread.Start();
        _userName = config.UserName;

        _client.Send(new byte[] { 0 }, 1);
    }

    public void ListenForMessages()
    {
        try
        {
            IPEndPoint remoteEP = new(IPAddress.Any, 0);
            while (true)
            {
                byte[] data = _client.Receive(ref remoteEP);
                if (data.Length <= 1)
                    continue;

                string jsonMessage = Encoding.UTF8.GetString(data);
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

        _client.Send(bytesMessage, bytesMessage.Length);

        return message;
    }

    public void Disconnect()
    {
        _client?.Close();
    }
}