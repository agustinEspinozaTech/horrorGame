using UnityEngine;

public enum BurnItemKind { Madera, ItemB, ItemC }

public class BurnItemPickup : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private BurnItemKind kind = BurnItemKind.Madera;
    [SerializeField] private string displayName = "Objeto de hoguera";

    [Header("Interacción")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string promptText = "Presiona E para recoger";
    [SerializeField] private float promptDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource pickSfx;

    [Header("Cámara")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    bool playerNear;
    bool collected;

    void Start()
    {
        if ((kind == BurnItemKind.Madera && HistoriaProgreso.hogueraMadera) ||
            (kind == BurnItemKind.ItemB && HistoriaProgreso.hogueraItemB) ||
            (kind == BurnItemKind.ItemC && HistoriaProgreso.hogueraItemC))
        {
            gameObject.SetActive(false);
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            displayName = kind.ToString();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!collected && other.CompareTag(playerTag))
        {
            playerNear = true;
            if (MessageUI.Instance != null) MessageUI.Instance.Show(promptText, promptDuration);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerNear = false;
            if (MessageUI.Instance != null) MessageUI.Instance.Hide();
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

        switch (kind)
        {
            case BurnItemKind.Madera:
                if (!HistoriaProgreso.hogueraMadera) HistoriaProgreso.hogueraMadera = true;
                break;
            case BurnItemKind.ItemB:
                if (!HistoriaProgreso.hogueraItemB) HistoriaProgreso.hogueraItemB = true;
                break;
            case BurnItemKind.ItemC:
                if (!HistoriaProgreso.hogueraItemC) HistoriaProgreso.hogueraItemC = true;
                break;
        }

        var hud = FindObjectOfType<BurnObjectiveHUD>();
        if (hud != null) hud.OnBurnItemCollected();
        else HistoriaProgreso.hogueraObjetosRecogidos++;

        if (MessageUI.Instance != null) MessageUI.Instance.Show($"{displayName} recogido", 2f);

        if (CameraShaker.Instance != null) CameraShaker.Instance.Shake(shakeDuration, shakeMagnitude);

        gameObject.SetActive(false);
    }
}
