using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField portInputField;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    void OnEnable()
    {
        hostButton.onClick.AddListener(() => OnStartChatPressed(ConnectionMode.Host));
        joinButton.onClick.AddListener(() => OnStartChatPressed(ConnectionMode.Client));
    }

    void OnDestroy()
    {
        hostButton.onClick.RemoveAllListeners();
        joinButton.onClick.RemoveAllListeners();
    }

    private void OnStartChatPressed(ConnectionMode mode)
    {
        string ip = "127.0.0.1";
        string userName = "Anónimo";

        if (mode == ConnectionMode.Client)
            ip = ipInputField.text;

        if (!int.TryParse(portInputField.text, out int port))
        {
            Debug.Log("No se pudo parsear el puerto");
            return;
        }

        if (!string.IsNullOrEmpty(userNameInputField.text))
            userName = userNameInputField.text;

        ConnectionConfig config = new(userName, ip, port, mode);
        ChatEvents.OnChatStartRequested?.Invoke(config);
    }
}
