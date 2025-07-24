using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance;

    public TextMeshProUGUI messageText;
    public float defaultDuration = 5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    public void Show(string text, float duration = -1f)
    {
        if (messageText == null || string.IsNullOrWhiteSpace(text)) return;

        messageText.text = text;
        messageText.gameObject.SetActive(true);

        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), duration > 0 ? duration : defaultDuration);
    }

    public void Hide()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }
}