using UnityEngine;

public class ReturnPointRestorer : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Start()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (!player) return;

        if (HistoriaProgreso.hasReturnPoint)
        {
            // Si usas CharacterController, desactívalo un momento para evitar choques con el suelo
            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.position = HistoriaProgreso.returnPos;
            player.rotation = Quaternion.Euler(HistoriaProgreso.returnEuler);

            if (cc) cc.enabled = true;

            HistoriaProgreso.hasReturnPoint = false;
        }
    }
}
