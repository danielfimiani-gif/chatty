public enum ConnectionMode
{
    Host,
    Client
}

public class ConnectionConfig
{
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public ConnectionMode Mode { get; private set; }

    public ConnectionConfig(string ip, int port, ConnectionMode mode)
    {
        this.Ip = ip;
        this.Port = port;
        this.Mode = mode;
    }

    public bool IsHost()
    {
        return Mode == ConnectionMode.Host;
    }
}
