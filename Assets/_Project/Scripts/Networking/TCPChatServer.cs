using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class TCPChatServer : IChatServer
{
    private TcpListener _listener;
    private List<TcpClient> _tcpClients;
    private Thread _thread;

    public void Start(ConnectionConfig config)
    {
        _tcpClients = new List<TcpClient>();
        _listener = new TcpListener(IPAddress.Any, config.Port);
        _listener.Start();

        _thread = new(AcceptClients)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    private void AcceptClients()
    {
        while (true)
        {
            try
            {
                TcpClient client = _listener.AcceptTcpClient();
                lock (_tcpClients)
                {
                    _tcpClients.Add(client);
                }
                Thread clientThread = new(() => HandleClient(client))
                {
                    IsBackground = true
                };
                clientThread.Start();
            }
            catch (System.Exception)
            {
                return;
            }
        }
    }

    private void HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
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

                BroadcastMessage(messageBuffer, client);
            }
        }
        catch (System.Exception)
        {
            lock (_tcpClients)
            {
                _tcpClients.Remove(client);
            }
            client.Close();
        }
    }

    private void BroadcastMessage(byte[] message, TcpClient sender)
    {
        byte[] messageLength = BitConverter.GetBytes(message.Length);
        lock (_tcpClients)
        {
            for (int i = _tcpClients.Count - 1; i >= 0; i--)
            {
                TcpClient client = _tcpClients[i];
                if (client != sender)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(messageLength, 0, messageLength.Length);
                        stream.Write(message, 0, message.Length);
                    }
                    catch (System.Exception)
                    {
                        client.Close();
                        _tcpClients.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
        lock (_tcpClients)
        {
            foreach (TcpClient client in _tcpClients)
            {
                client.Close();
            }
        }
        _tcpClients.Clear();
    }
}