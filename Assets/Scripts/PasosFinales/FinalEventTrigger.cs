using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class FinalEventTrigger : MonoBehaviour
{
    [Header("Configuración Mensaje")]
    [SerializeField] private string promptMessage = "Presione E para tirar los objetos";
    [SerializeField] private float messageDuration = 4f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Animación y Control")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private string animTrigger = "Terrified";
    [SerializeField] private AudioSource audioSource;

    [Header("Efectos")]
    [SerializeField] private float cameraShakeDuration = 5f;
    [SerializeField] private float cameraShakeMagnitude = 0.7f;

    [Header("Escena Final")]
    [SerializeField] private string nextSceneName = "ConclusionFinal";

    bool playerInRange = false;
    bool finalActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !finalActivated)
        {
            playerInRange = true;
            MessageUI.Instance?.Show(promptMessage, messageDuration);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !finalActivated)
        {
            playerInRange = false;
            MessageUI.Instance?.Hide();
        }
    }

    void Update()
    {
        if (playerInRange && !finalActivated && Input.GetKeyDown(interactKey))
        {
            finalActivated = true;
            MessageUI.Instance?.Hide();
            StartCoroutine(FinalSequence());
        }
    }

    IEnumerator FinalSequence()
    {
        AudioController.bloqueadoPorAudio = true;

        if (playerAnimator != null)
            playerAnimator.SetBool(animTrigger, true);

        if (CameraShaker.Instance != null)
            CameraShaker.Instance.Shake(cameraShakeDuration, cameraShakeMagnitude);

        if (audioSource != null)
            audioSource.Play();

        // Espera 3.5 segundos antes de cambiar de escena
        yield return new WaitForSeconds(3.5f);

        if (playerAnimator != null)
            playerAnimator.SetBool(animTrigger, false);

        AudioController.bloqueadoPorAudio = false;

        SceneManager.LoadScene(nextSceneName);
    }
}
