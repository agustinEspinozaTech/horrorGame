using UnityEngine;

public class EnemyPersistenceFlagger : MonoBehaviour
{
    void OnEnable()
    {
        // Marca que, si sales y vuelves a entrar a la escena, el enemigo debe estar activo
        HistoriaProgreso.enemigoDebePersistir = true;
    }
}
