using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExitLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NarrativaFinal";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float loadDelay = 0f;

    [Header("Asignaciones")]
    [SerializeField] private Transform player;               // opcional: puedes dejarlo vacío
    [SerializeField] private Transform returnPointOverride;  //  arrastra aquí tu pointReturn (afuera)

    bool fired;

    void Start()
    {
        if (HistoriaProgreso.narrativaFinalMostrada)
        {
            var col = GetComponent<Collider>();
            if (col) col.enabled = false;
            enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (fired) return;
        if (!other.CompareTag(playerTag)) return;
        if (HistoriaProgreso.narrativaFinalMostrada) return;

        // fallback: si no asignaste player, uso el que entra
        if (!player) player = other.transform;

        fired = true;
        HistoriaProgreso.narrativaFinalMostrada = true;

        // Guardar punto de retorno: usa el override si está asignado (recomendado)
        HistoriaProgreso.hasReturnPoint = true;
        if (returnPointOverride != null)
        {
            HistoriaProgreso.returnPos = returnPointOverride.position;
            HistoriaProgreso.returnEuler = returnPointOverride.eulerAngles;
        }
        else if (player != null)
        {
            HistoriaProgreso.returnPos = player.position;
            HistoriaProgreso.returnEuler = player.eulerAngles;
        }

        var col = GetComponent<Collider>();
        if (col) col.enabled = false;

        if (loadDelay > 0f) Invoke(nameof(LoadScene), loadDelay);
        else LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
