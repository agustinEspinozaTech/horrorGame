using UnityEngine;

public class ExitDoorSeal : MonoBehaviour
{
    [Header("Mensaje al jugador")]
    [SerializeField, TextArea(2, 4)]
    private string blockedMessage = "Una fuerza invisible bloquea la salida… El ritual sigue activo. Encuentra los tres objetos usados para sellar la casa y destrúyelos.";
    [SerializeField] private float messageDuration = 5f;
    [SerializeField] private string interactPrompt = "Presiona E para intentar abrir la puerta";

    [Header("Audio sobrenatural")]
    [SerializeField] private AudioSource sfx;
    [SerializeField] private bool playOneShot = true;

    [Header("Interacción")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";

    [Header("Objetos del ritual (libro, cruz, vela)")]
    [SerializeField] private GameObject[] ritualObjects; // arrastra acá Libro, Cruz, Vela

    private bool playerNear;
    private bool alreadyAnnounced;

    void Start()
    {
        // Ocultos al inicio
        if (ritualObjects != null)
            for (int i = 0; i < ritualObjects.Length; i++)
                if (ritualObjects[i] != null) ritualObjects[i].SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerNear = true;
            if (MessageUI.Instance != null)
                MessageUI.Instance.Show(interactPrompt, messageDuration);
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
        if (!playerNear) return;
        if (!Input.GetKeyDown(interactKey)) return;

        if (HistoriaProgreso.cintaReproducida &&
            HistoriaProgreso.cartaDestruida &&
            HistoriaProgreso.fotografiaDestruida)
        {
            if (!alreadyAnnounced)
            {
                if (sfx != null)
                {
                    if (playOneShot && sfx.clip != null) sfx.PlayOneShot(sfx.clip);
                    else sfx.Play();
                }

                if (MessageUI.Instance != null)
                    MessageUI.Instance.Show($"<color=#FFFFFF>{blockedMessage}</color>", messageDuration);

                // Activar los objetos del ritual
                if (ritualObjects != null)
                    for (int i = 0; i < ritualObjects.Length; i++)
                        if (ritualObjects[i] != null) ritualObjects[i].SetActive(true);

                alreadyAnnounced = true;
            }
            else
            {
                if (MessageUI.Instance != null)
                    MessageUI.Instance.Show($"<color=#FFFFFF>{blockedMessage}</color>", messageDuration);
            }
        }
    }
}
