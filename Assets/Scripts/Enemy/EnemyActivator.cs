using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    public KeyPickup keyPickup;         // Referencia al script de la llave
    public GameObject enemyToActivate;  // Enemigo a activar
    public float delaySeconds = 2f;     // Tiempo de espera antes de aparecer

    private bool hasActivated = false;

    void Update()
    {
        if (!hasActivated && keyPickup.HasBeenCollected())
        {
            hasActivated = true;
            StartCoroutine(ActivateEnemyWithDelay());
        }
    }

    private IEnumerator ActivateEnemyWithDelay()
    {
        yield return new WaitForSeconds(delaySeconds);

        if (enemyToActivate != null)
        {
            enemyToActivate.SetActive(true);
        }
    }
}
