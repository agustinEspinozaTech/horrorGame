using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;


public class TypewriterEffectNarrativa : MonoBehaviour
{
    [Header("Texto y configuración")]
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    public string[] lines;
    public float typingSpeed = 0.08f;

    [Header("Cambio de escena")]
    public ChangeSceneAfterTypewriter changeSceneScript;

    private int index = 0;

    void Start()
    {
        textComponent.text = "";
        StartCoroutine(TypeLine());
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
            StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = "";

            // Llamamos al script que cambia de escena
            if (changeSceneScript != null)
            {
                changeSceneScript.CargarSiguienteEscena();
            }
          
        }
    }
}