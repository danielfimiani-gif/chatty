public enum ConnectionMode
{
    Host,
    Client
}

public enum TransportProtocol
{
    TCP,
    UDP
}

public class ConnectionConfig
{
    public string UserName { get; private set; }
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public ConnectionMode Mode { get; private set; }

    public ConnectionConfig(string userName, string ip, int port, ConnectionMode mode)
    {
        this.UserName = userName;
        this.Ip = ip;
        this.Port = port;
        this.Mode = mode;
    }

    public bool IsHost()
    {
        return Mode == ConnectionMode.Host;
    }
}
