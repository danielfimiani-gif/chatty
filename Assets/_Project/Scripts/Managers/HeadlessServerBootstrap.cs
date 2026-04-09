using System;
using UnityEngine;

class HeadlessServerBootstrap : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    private string _userName = "HeadlessServer";
    private TransportProtocol _protocol = TransportProtocol.TCP;
    private int _port = 7777;

    private IChatServer _chatServer;

    void Awake()
    {
        if (!Application.isBatchMode)
        {
            enabled = false;
            return;
        }

        StartServer();
    }

    void OnDestroy()
    {
        _chatServer?.Stop();
    }

    private void StartServer()
    {
        UI.SetActive(false);

        GetConnectionArgs();

        _chatServer = _protocol == TransportProtocol.TCP ?
            new TCPChatServer() : new UDPChatServer();

        ConnectionConfig config = new(_userName, null, _port, ConnectionMode.Host);

        _chatServer.Start(config);

        Debug.Log($"Servidor configurado: {_protocol} en puerto {_port}");
    }

    private void GetConnectionArgs()
    {
        string[] args = Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-port" && i + 1 < args.Length)
            {
                int.TryParse(args[i + 1], out _port);
            }

            if (args[i] == "-protocol" && i + 1 < args.Length)
            {
                string val = args[i + 1];
                if (Enum.TryParse(val, true, out TransportProtocol parsedProto))
                {
                    _protocol = parsedProto;
                }
                else
                {
                    Debug.LogWarning($"Protocolo '{val}' no reconocido. Usando {_protocol}");
                }
            }
        }
    }
}