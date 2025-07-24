using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class HouseEntryText : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)] public string[] lines;
    public float typingSpeed = 0.04f;

    void Start()
    {
        UnityEngine.Debug.Log("HouseEntryText: Start() ejecutado.");
        StartCoroutine(ShowTextSequence());
    }

    IEnumerator ShowTextSequence()
    {
        UnityEngine.Debug.Log("HouseEntryText: Comenzando ShowTextSequence");
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        // Fade in canvas
        float fadeInTime = 1f;
        float t = 0;
        while (t < fadeInTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        UnityEngine.Debug.Log("HouseEntryText: Fade in completo. Mostrando líneas...");

        textComponent.text = "";
        for (int i = 0; i < lines.Length; i++)
        {
            foreach (char c in lines[i].ToCharArray())
            {
                textComponent.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(2f);
            textComponent.text = "";
        }
        UnityEngine.Debug.Log("HouseEntryText: Terminó de mostrar texto. Fade out...");

        // Fade out canvas
        t = 0;
        while (t < fadeInTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeInTime);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
        UnityEngine.Debug.Log("HouseEntryText: Texto ocultado.");
    }
}
