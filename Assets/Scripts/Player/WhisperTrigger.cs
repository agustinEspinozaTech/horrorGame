using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTrigger : MonoBehaviour
{
    public Animator playerAnimator;
    public AudioSource whisperAudio;
    public string triggerName = "WhisperReaction";
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (whisperAudio != null)
            {
                whisperAudio.Play();
                Invoke(nameof(StopWhisper), 4f); // Detener después de 2 segundos
            }

            if (playerAnimator != null)
                playerAnimator.SetTrigger(triggerName);
        }
    }

    private void StopWhisper()
    {
        if (whisperAudio != null && whisperAudio.isPlaying)
        {
            whisperAudio.Stop();
        }
    }
}
