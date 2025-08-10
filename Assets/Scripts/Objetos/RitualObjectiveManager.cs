using UnityEngine;

public class RitualObjectiveManager : MonoBehaviour
{
    [SerializeField] private int requiredCount = 3;
    [SerializeField] private string allDoneMessage = "El sello se debilita... La salida podría estar libre.";
    [SerializeField] private float allDoneMessageDuration = 3f;

    private int collectedCount = 0;

    public void OnItemCollected(string itemName)
    {
        collectedCount++;
        // print($"Recogido: {itemName} ({collectedCount}/{requiredCount})"); // usar solo si necesitás depurar

        if (collectedCount >= requiredCount)
        {
            if (MessageUI.Instance != null)
                MessageUI.Instance.Show(allDoneMessage, allDoneMessageDuration);

            // Aquí, en el siguiente paso, desbloqueamos la puerta.
            // Por ahora solo avisamos al jugador.
        }
    }
}
