using UnityEngine;

public class RitualObjectiveManager : MonoBehaviour
{
    [Header("Requerimientos")]
    [SerializeField] private int requiredCount = 3;
    [SerializeField] private string allDoneMessage = "El sello se debilita... La salida podría estar libre.";
    [SerializeField] private float allDoneMessageDuration = 3f;

    [Header("Puerta")]
    [SerializeField] private GameObject exitDoor; // asignar la puerta en el Inspector
    [SerializeField] private AudioSource openDoorSfx; // sonido de abrir puerta

    private int collectedCount = 0;

    public void OnItemCollected(string itemName)
    {
        collectedCount++;

        if (collectedCount >= requiredCount)
        {
            if (MessageUI.Instance != null)
                MessageUI.Instance.Show(allDoneMessage, allDoneMessageDuration);

            // Reproducir sonido de abrir puerta
            if (openDoorSfx != null)
            {
                if (openDoorSfx.clip != null) openDoorSfx.PlayOneShot(openDoorSfx.clip);
                else openDoorSfx.Play();
            }

            // Desactivar puerta en escena
            if (exitDoor != null) exitDoor.SetActive(false);
            HistoriaProgreso.puertaSalidaDesactivada = true;
            HistoriaProgreso.enemigoDebePersistir = true;
        }
    }
}
