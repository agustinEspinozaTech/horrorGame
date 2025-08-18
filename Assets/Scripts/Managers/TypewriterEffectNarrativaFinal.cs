using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TypewriterEffectNarrativaFinal : MonoBehaviour
{
    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField, TextArea(3, 10)] private string[] lines;
    [SerializeField] private float typingSpeed = 0.08f;
    [SerializeField] private float betweenLinesDelay = 2.5f;

    [Header("Escena a cargar al finalizar")]
    [SerializeField] private string sceneToLoad = "DestruirEvidencia";

    private int index;
    private bool isTyping;
    private bool isWaitingDelay;
    private Coroutine typingCoroutine;
    private Coroutine delayCoroutine;

    void Start()
    {
        if (!textComponent) return;
        textComponent.text = string.Empty;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                textComponent.text = lines[index];
                isTyping = false;
                delayCoroutine = StartCoroutine(PostLineDelay());
            }
            else if (isWaitingDelay)
            {
                if (delayCoroutine != null) StopCoroutine(delayCoroutine);
                isWaitingDelay = false;
                NextLine();
            }
        
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        delayCoroutine = StartCoroutine(PostLineDelay());
    }

    IEnumerator PostLineDelay()
    {
        isWaitingDelay = true;
        yield return new WaitForSeconds(betweenLinesDelay);
        isWaitingDelay = false;
        NextLine();
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (delayCoroutine != null) StopCoroutine(delayCoroutine);

            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty;
            HistoriaProgreso.hogueraObjetivoActivo = true;
            HistoriaProgreso.hogueraObjetosRecogidos = 0;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
