using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class UDPChatServer : IChatServer
{
    private UdpClient _client;
    private List<IPEndPoint> _udpClients;
    private Thread _thread;

    public void Start(ConnectionConfig config)
    {
        _udpClients = new List<IPEndPoint>();
        _client = new UdpClient(config.Port);

        _thread = new(Receive)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    private void Receive()
    {
        try
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] data = _client.Receive(ref remoteEP);
                if (!_udpClients.Contains(remoteEP))
                    _udpClients.Add(remoteEP);

                if (data.Length > 1)
                    Broadcast(data, remoteEP);
            }
        }
        catch (System.Exception)
        {
            _client.Close();
        }
    }

    private void Broadcast(byte[] data, IPEndPoint sender)
    {
        foreach (IPEndPoint remoteEP in _udpClients)
        {
            if (!remoteEP.Equals(sender))
                _client.Send(data, data.Length, remoteEP);
        }
    }

    public void Stop()
    {
        _client.Close();
        _udpClients.Clear();
    }
}