using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;


public class TypewriterEffectNarrativa : MonoBehaviour
{
    [Header("Texto y configuración")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    [SerializeField] private string[] lines;
    [SerializeField] private float typingSpeed = 0.08f;

    [Header("Cambio de escena")]
    [SerializeField] private ChangeSceneAfterTypewriter changeSceneScript;

    private int index = 0;
    private bool isTyping;
    private Coroutine typingCoroutine;

    void Start()
    {
        textComponent.text = "";
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Si está escribiendo => mostrar línea completa de golpe
                StopCoroutine(typingCoroutine);
                textComponent.text = lines[index];
                isTyping = false;
            }
            else
            {
                // Si ya terminó => avanzar de inmediato
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        yield return new WaitForSeconds(2.5f);
        NextLine();
    }

    void NextLine()
    {
        index++;
        if (index < lines.Length)
        {
            textComponent.text = "";
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = "";
            if (changeSceneScript != null)
            {
                changeSceneScript.CargarSiguienteEscena();
            }
        }
    }
}