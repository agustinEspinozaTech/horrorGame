using System.Collections;
using UnityEngine;

public class PuertaSimpleInteractiva : MonoBehaviour
{
    [Header("Mensaje del jugador")]
    [SerializeField] private string mensajeJugador = "Parece que está cerrada. No puedo abrirla.";
    [SerializeField] private float duracionMensaje = 4f;

    [Header("Cartel de interacción")]
    [SerializeField] private GameObject cartelInteractuar; // Texto flotante "[E] Interactuar"

    private bool jugadorCerca = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !mensajeMostrado)
        {
       
            MessageUI.Instance.Show(mensajeJugador, duracionMensaje);
            mensajeMostrado = true;

            if (cartelInteractuar != null)
                cartelInteractuar.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            mensajeMostrado = false;

            MessageUI.Instance.Show("Presiona 'E' para intentar abrir la puerta");

            if (cartelInteractuar != null)
                cartelInteractuar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            mensajeMostrado = false;

            MessageUI.Instance.Hide();

            if (cartelInteractuar != null)
                cartelInteractuar.SetActive(false);
        }
    }
}
