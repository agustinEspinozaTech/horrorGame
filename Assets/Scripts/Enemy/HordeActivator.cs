using System.Collections.Generic;
using UnityEngine;

public class HordeActivator : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private List<GameObject> enemiesHidden = new List<GameObject>();

    [Header("Aparición alrededor del jugador")]
    [SerializeField] private bool repositionAroundPlayer = true;
    [SerializeField] private float radius = 6f;

    [Header("Altura fija opcional")]
    [SerializeField] private bool forceYFromEnemies = true;

    Transform player;

    public void ActivateHorde()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) player = p.transform;
        }
        if (enemiesHidden == null || enemiesHidden.Count == 0) return;

        for (int i = 0; i < enemiesHidden.Count; i++)
        {
            var e = enemiesHidden[i];
            if (e == null) continue;

            if (repositionAroundPlayer && player != null)
            {
                float angle = (360f / Mathf.Max(1, enemiesHidden.Count)) * i * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;

                Vector3 pos = player.position + offset;
                if (forceYFromEnemies)
                    pos.y = e.transform.position.y;

                e.transform.position = pos;
                e.transform.LookAt(new Vector3(player.position.x, e.transform.position.y, player.position.z));
            }

            e.SetActive(true); // EnemyProximityTrigger hará el resto
        }
    }
}
