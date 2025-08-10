using UnityEngine;

public class RitualItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "Objeto del ritual"; // Libro / Cruz / Vela
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string promptText = "Presiona E para recoger";
    [SerializeField] private float promptDuration = 2.5f;
    [SerializeField] private AudioSource pickSfx; // opcional

    // Configuración del temblor de cámara
    [Header("Efecto de cámara")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private bool playerNear;
    private bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !collected)
        {
            playerNear = true;
            if (MessageUI.Instance != null)
                MessageUI.Instance.Show(promptText, promptDuration);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerNear = false;
            if (MessageUI.Instance != null)
                MessageUI.Instance.Hide();
        }
    }

    void Update()
    {
        if (!playerNear || collected) return;
        if (!Input.GetKeyDown(interactKey)) return;

        collected = true;

        if (pickSfx != null)
        {
            if (pickSfx.clip != null) pickSfx.PlayOneShot(pickSfx.clip);
            else pickSfx.Play();
        }

        if (MessageUI.Instance != null)
            MessageUI.Instance.Show($"{itemName} recogido", 2f);

        var manager = FindFirstObjectByType<RitualObjectiveManager>();
        if (manager != null)
            manager.OnItemCollected(itemName);

        //  Agregar temblor de cámara
        if (CameraShaker.Instance != null)
            CameraShaker.Instance.Shake(shakeDuration, shakeMagnitude);

        gameObject.SetActive(false);
    }
}
