using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationReminder : MonoBehaviour
{
    public KeyPickup keyPickup; // Referencia al script de la llave
    public float initialDelay = 60f; // Tiempo antes del primer mensaje
    public float interval = 45f;     // Tiempo entre mensajes
    private int messageIndex = 0;
    private bool stopMessages = false;

    private string[] messages = new string[]
    {
        "Algo no está bien...\nNo debería quedarme aquí mucho tiempo.",
        "¿Por qué me siento observado?\nDebo encontrar esa llave rápido.",
        "El aire se vuelve más denso...\n¿Y si ya no estoy solo?",
        "No puedo perder más tiempo.\nEsa llave tiene que estar cerca.",
        "Esto no es solo una casa vacía...\nEs como si supiera que volví."
    };

    void Start()
    {
        StartCoroutine(ShowReminders());
    }

    IEnumerator ShowReminders()
    {
        yield return new WaitForSeconds(initialDelay);

        while (!stopMessages && messageIndex < messages.Length)
        {
            if (keyPickup == null || !keyPickup.HasBeenCollected())
            {
                MessageUI.Instance.Show(messages[messageIndex], 5f);
                messageIndex++;
                yield return new WaitForSeconds(interval);
            }
            else
            {
                stopMessages = true;
                yield break;
            }
        }
    }
}
