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

    int index;

    void Start()
    {
        if (!textComponent) return;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(betweenLinesDelay);
        index++;

        if (index < lines.Length)
        {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
