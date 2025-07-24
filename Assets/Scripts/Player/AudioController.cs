using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource backgroundAudio;     // El AudioSource de fondo
    public AudioSource whisperAudio;        // El AudioSource del susurro
    public float volumeDuringWhisper = 0f;  // Silencio temporal
    public float whisperDuration = 6f;      // Duración del susurro
    public float originalVolume = 0.05f;    // Volumen original del fondo

    public static bool bloqueadoPorAudio = false;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed || !other.CompareTag("Player")) return;
        hasPlayed = true;

        StartCoroutine(PlayWhisperEffect());
    }

    IEnumerator PlayWhisperEffect()
    {
        // Bloquear movimiento
        bloqueadoPorAudio = true;

        // Bajar volumen de fondo
        if (backgroundAudio != null)
            backgroundAudio.volume = volumeDuringWhisper;

        // Reproducir susurro
        if (whisperAudio != null)
            whisperAudio.Play();

       

        // Esperar duración del susurro
        yield return new WaitForSeconds(whisperDuration);

        //  Desbloquear movimiento
        bloqueadoPorAudio = false;

        // Mostrar mensaje en pantalla
        MessageUI.Instance.Show("¿Qué fue eso? Ese sonido fue aterrador...", 4f);

        // Restaurar volumen original
        if (backgroundAudio != null)
            backgroundAudio.volume = originalVolume;

        // Esperar 3 segundos antes de mostrar el siguiente mensaje
        yield return new WaitForSeconds(6f);

        MessageUI.Instance.Show("Desde que llegué, siento que algo no quiere que esté aquí...\nCreo que estoy cerca de la llave.", 5f);

     
    }
    public void TemporarilyLowerBackground(float duration)
    {
        CoroutineRunner.Instance.StartCoroutine(LowerBackgroundForDuration(duration));

    }

    private IEnumerator LowerBackgroundForDuration(float duration)
    {
        if (backgroundAudio != null)
            backgroundAudio.volume = volumeDuringWhisper;

        yield return new WaitForSeconds(duration);

        if (backgroundAudio != null)
            backgroundAudio.volume = originalVolume;
    }
}
