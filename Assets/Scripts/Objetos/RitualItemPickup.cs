using UnityEngine;

public class RitualItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "Objeto del ritual"; // Libro / Cruz / Vela
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string promptText = "Presiona E para recoger";
    [SerializeField] private float promptDuration = 2.5f;
    [SerializeField] private AudioSource pickSfx; // opcional

    [Header("Efecto de cámara")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private bool playerNear;
    private bool collected;

    void Start()
    {
        string n = itemName.ToLower();
        if ((n == "libro" && HistoriaProgreso.libroRecogido) ||
            (n == "cruz" && HistoriaProgreso.cruzRecogida) ||
            (n == "vela" && HistoriaProgreso.velaRecogida))
        {
            gameObject.SetActive(false);
        }
    }

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

        // Sonido de recogida
        if (pickSfx != null)
        {
            if (pickSfx.clip != null) pickSfx.PlayOneShot(pickSfx.clip);
            else pickSfx.Play();
        }

        // Mensaje de recogida
        if (MessageUI.Instance != null)
            MessageUI.Instance.Show($"{itemName} recogido", 2f);

        // Notificar al manager (si existe)
        var manager = FindFirstObjectByType<RitualObjectiveManager>();
        if (manager != null)
            manager.OnItemCollected(itemName);

        // Guardar en HistoriaProgreso qué objeto se recogió
        switch (itemName.ToLower())
        {
            case "libro":
                HistoriaProgreso.libroRecogido = true;
                break;
            case "cruz":
                HistoriaProgreso.cruzRecogida = true;
                break;
            case "vela":
                HistoriaProgreso.velaRecogida = true;
                break;
        }

        // Temblor de cámara
        if (CameraShaker.Instance != null)
            CameraShaker.Instance.Shake(shakeDuration, shakeMagnitude);

        // Ocultar el objeto
        gameObject.SetActive(false);
    }
}
