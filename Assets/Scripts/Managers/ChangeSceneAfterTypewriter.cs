using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterTypewriter : MonoBehaviour
{
    [Header("Evidencia")]
    [SerializeField] private string nombreEscena = "DestruirEvidencia";

    [Header("Tiempo de espera antes de cambiar")]
    [SerializeField] private float delayAntesDeCambiar = 1.5f;

    /// <summary>
    /// Llama esta función para cambiar de escena con retardo.
    /// </summary>
    public void CargarSiguienteEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            StartCoroutine(CambiarConDelay());
        }
        else
        {
            print("No se ha asignado un nombre de escena para cargar.");
        }
    }

    private IEnumerator CambiarConDelay()
    {
        yield return new WaitForSeconds(delayAntesDeCambiar);
        SceneManager.LoadScene(nombreEscena);
 
    }
}
