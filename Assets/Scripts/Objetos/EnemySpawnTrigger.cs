using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [Header("Referencia al enemigo a activar")]
    [SerializeField] private GameObject enemigo;

    private bool activado = false;

    void Update()
    {
        if (activado) return;

        if (HistoriaProgreso.cintaReproducida &&
            HistoriaProgreso.cartaDestruida &&
            HistoriaProgreso.fotografiaDestruida)
        {
            activado = true;
            ActivarEnemigo();
        }
    }

    private void ActivarEnemigo()
    {
        if (enemigo != null)
        {
            enemigo.SetActive(true);
            print("El enemigo fue activado al recolectar los 3 objetos.");
        }
    }
}
