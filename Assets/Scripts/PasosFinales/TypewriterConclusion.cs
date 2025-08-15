using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class TypewriterConclusion : MonoBehaviour
{
    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField, TextArea(3, 10)] private string[] lines;
    [SerializeField] private float typingSpeed = 0.08f;
    [SerializeField] private float betweenLinesDelay = 2.0f;

    [Header("Comportamiento")]
    [SerializeField] private bool allowSkipCurrentLine = true;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;

    [Header("UI Final")]
    [SerializeField] private GameObject finalPanel;     // Debe tener CanvasGroup
    [SerializeField] private Button exitButton;

    [Header("Efectos")]
    [SerializeField] private float fadeOutTextDuration = 0.5f;
    [SerializeField] private float fadeInPanelDuration = 0.8f;

    [Header("Opcional")]
    [SerializeField] private bool showCursorAtEnd = true;
    [SerializeField] private AudioSource tickSfx;

    private int index = 0;
    private bool isTyping = false;
    private bool skipRequested = false;
    private CanvasGroup panelCg;

    void Start()
    {
        if (!textComponent) return;

        Time.timeScale = 1f;

        if (finalPanel)
        {
            panelCg = finalPanel.GetComponent<CanvasGroup>();
            if (panelCg == null) panelCg = finalPanel.AddComponent<CanvasGroup>();
            finalPanel.SetActive(false);          // lo activamos al final
            panelCg.alpha = 0f;
            panelCg.interactable = false;
            panelCg.blocksRaycasts = false;
        }

        textComponent.text = string.Empty;

        if (exitButton)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnClickSalir);
        }

        StartCoroutine(Run());
    }

    void Update()
    {
        if (allowSkipCurrentLine && isTyping && Input.GetKeyDown(skipKey))
            skipRequested = true;
    }

    IEnumerator Run()
    {
        while (index < lines.Length)
        {
            yield return StartCoroutine(TypeLine(lines[index]));
            index++;
            if (index < lines.Length)
                yield return new WaitForSeconds(betweenLinesDelay);
        }

        // 1) Desvanece el texto
        yield return StartCoroutine(FadeTMP(textComponent, 1f, 0f, fadeOutTextDuration));
        textComponent.text = string.Empty; // lo dejas limpio

        // 2) Muestra y desvanece el panel/botón
        ShowFinalUI();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        skipRequested = false;
        textComponent.text = string.Empty;

        // aseguro alpha a 1 por si venimos de otro estado
        var c = textComponent.color; c.a = 1f; textComponent.color = c;

        foreach (char ch in line)
        {
            if (skipRequested)
            {
                textComponent.text = line;
                break;
            }

            textComponent.text += ch;
            if (tickSfx) tickSfx.Play();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowFinalUI()
    {
        if (showCursorAtEnd)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!finalPanel || panelCg == null) return;

        finalPanel.SetActive(true);
        panelCg.alpha = 0f;
        panelCg.interactable = false;
        panelCg.blocksRaycasts = false;

        StartCoroutine(FadeCanvasGroup(panelCg, 0f, 1f, fadeInPanelDuration, enableAtEnd: true));
    }

    // Botón Salir
    public void OnClickSalir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    // ---- Helpers de fade ----
    IEnumerator FadeTMP(TextMeshProUGUI tmp, float from, float to, float duration)
    {
        if (!tmp) yield break;
        float t = 0f;
        Color baseColor = tmp.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            yield return null;
        }
        tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, to);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration, bool enableAtEnd)
    {
        if (!cg) yield break;
        float t = 0f;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
        cg.interactable = enableAtEnd;
        cg.blocksRaycasts = enableAtEnd;
    }
}
