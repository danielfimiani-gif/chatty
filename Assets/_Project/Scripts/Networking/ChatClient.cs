using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatClient
{
    private TcpClient _client;
    private Thread _thread;
    public event Action<string> OnMessageReceived;

    public void Connect(string ip, int port)
    {
        _client = new TcpClient();
        _client.Connect(ip, port);
        _thread = new Thread(ListenForMessages)
        {
            IsBackground = true,
        };
        _thread.Start();
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

                string message = Encoding.UTF8.GetString(messageBuffer);
                OnMessageReceived?.Invoke(message);
            }
        }
        catch (System.Exception)
        {
            _client?.Close();
        }

    }

    public void SendMessage(string message)
    {
        byte[] bytesMessage = Encoding.UTF8.GetBytes(message);
        byte[] lengthBytes = BitConverter.GetBytes(bytesMessage.Length);

        NetworkStream stream = _client.GetStream();
        stream.Write(lengthBytes, 0, lengthBytes.Length);
        stream.Write(bytesMessage, 0, bytesMessage.Length);
    }

    public void Disconnect()
    {
        _client?.Close();
    }
}