using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    public string[] lines;
    public float typingSpeed = 0.08f;
    public GameObject menuPanel;
    public Button skipButton;

    private int index = 0;
    private Coroutine typingCoroutine;

    private CanvasGroup skipButtonGroup;

    void Start()
    {
        textComponent.text = "";

        skipButtonGroup = skipButton.GetComponent<CanvasGroup>();
        if (skipButtonGroup == null)
        {
            skipButtonGroup = skipButton.gameObject.AddComponent<CanvasGroup>();
        }

        skipButtonGroup.alpha = 0;
        skipButton.gameObject.SetActive(true);
        skipButton.onClick.AddListener(SkipIntro);

        StartCoroutine(FadeInSkipButton());
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator FadeInSkipButton()
    {
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            skipButtonGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        skipButtonGroup.alpha = 1f;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2.5f);
        index++;

        if (index < lines.Length)
        {
            textComponent.text = "";
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = "";
            skipButtonGroup.alpha = 0;
            skipButton.interactable = false;
            skipButton.gameObject.SetActive(false);
            StartCoroutine(FadeInMenu());
        }
    }

    void SkipIntro()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textComponent.text = "";

        skipButtonGroup.alpha = 0;
        skipButton.interactable = false;
        skipButton.gameObject.SetActive(false);

        StartCoroutine(FadeInMenu());
    }

    IEnumerator FadeInMenu()
    {
        CanvasGroup canvasGroup = menuPanel.GetComponent<CanvasGroup>();
        menuPanel.SetActive(true);
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
