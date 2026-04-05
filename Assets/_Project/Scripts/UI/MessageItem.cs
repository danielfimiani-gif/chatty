using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MessageItem : MonoBehaviour
{
    [SerializeField] private GameObject replyBlock;
    [SerializeField] private TMP_Text replyText;
    [SerializeField] private TMP_Text messageText;

    private ChatMessage _chatMessage;
    private LayoutElement _bubbleLayout;
    private HorizontalLayoutGroup _parentLayout;

    private void Awake()
    {
        _bubbleLayout = GetComponent<LayoutElement>();
        _parentLayout = transform.parent.GetComponent<HorizontalLayoutGroup>();
    }

    public void Setup(ChatMessage message, string replyPreview, bool isMine)
    {
        _chatMessage = message;
        messageText.text = message.Format();

        float parentWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        _bubbleLayout.preferredWidth = parentWidth * 0.75f;

        if (!string.IsNullOrEmpty(replyPreview))
        {
            replyBlock.SetActive(true);
            replyText.text = replyPreview;
        }
        else
        {
            replyBlock.SetActive(false);
        }

        if (_parentLayout != null)
        {
            if (isMine)
            {
                _parentLayout.childAlignment = TextAnchor.MiddleRight;
                _parentLayout.padding.left = 50;
                _parentLayout.padding.right = 10;
            }
            else
            {
                _parentLayout.childAlignment = TextAnchor.MiddleLeft;
                _parentLayout.padding.left = 10;
                _parentLayout.padding.right = 50;
            }
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    private void OnClick()
    {
        ChatEvents.OnReplyMessageRequested?.Invoke(_chatMessage);
    }
}
