using TMPro;
using UnityEngine;

public class MessageItem : MonoBehaviour
{
    [SerializeField] private GameObject replyBlock;
    [SerializeField] private TMP_Text replyText;
    [SerializeField] private TMP_Text messageText;

    private ChatMessage _chatMessage;

    public void Setup(ChatMessage message, string replyPreview)
    {
        _chatMessage = message;
        messageText.text = message.Format();
        if (replyPreview is not null)
        {
            replyBlock.SetActive(true);
            replyText.text = replyPreview;
        }
    }

    public void OnClick()
    {
        ChatEvents.OnReplyMessageRequested?.Invoke(_chatMessage);
    }
}
