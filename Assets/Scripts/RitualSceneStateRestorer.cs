using UnityEngine;

public class RitualSceneStateRestorer : MonoBehaviour
{
    [Header("Orden: 0=Libro, 1=Cruz, 2=Vela")]
    [SerializeField] private GameObject[] ritualObjects = new GameObject[3];
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private GameObject enemyRoot;         // raíz del enemigo a mantener
    [SerializeField] private Transform enemySpawnPoint;    // opcional

    void Start()
    {
        // Puerta
        if (HistoriaProgreso.puertaSalidaDesactivada && exitDoor)
            exitDoor.SetActive(false);

        // Ritual activado previamente
        if (HistoriaProgreso.ritualActivado)
        {
            // Mostrar todos los objetos del ritual
            for (int i = 0; i < ritualObjects.Length; i++)
                if (ritualObjects[i]) ritualObjects[i].SetActive(true);

            // Ocultar los ya recogidos
            if (HistoriaProgreso.libroRecogido && ritualObjects.Length > 0 && ritualObjects[0])
                ritualObjects[0].SetActive(false);

            if (HistoriaProgreso.cruzRecogida && ritualObjects.Length > 1 && ritualObjects[1])
                ritualObjects[1].SetActive(false);

            if (HistoriaProgreso.velaRecogida && ritualObjects.Length > 2 && ritualObjects[2])
                ritualObjects[2].SetActive(false);
        }
        else
        {
            // Si nunca se activó el ritual, asegúrate de que estén ocultos
            for (int i = 0; i < ritualObjects.Length; i++)
                if (ritualObjects[i]) ritualObjects[i].SetActive(false);
        }

        // Enemigo persistente
        if (HistoriaProgreso.enemigoDebePersistir && enemyRoot)
        {
            enemyRoot.SetActive(true);

            if (enemySpawnPoint)
            {
                enemyRoot.transform.position = enemySpawnPoint.position;
                enemyRoot.transform.rotation = enemySpawnPoint.rotation;
            }
        }
    }
}
