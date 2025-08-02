using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PuertaBloqueada : MonoBehaviour
{
    [Header("Mensaje de la puerta bloqueada")]
    [SerializeField] private string mensaje = "�Por qu� no se abre?\nQuiero irme... quiero salir de aqu�.";
    [SerializeField] private float duracionMensaje = 5f;

    private bool jugadorCerca = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            print("Jugador presion� E cerca de la puerta.");

            if (HistoriaProgreso.cintaReproducida && !mensajeMostrado)
            {
                print("Cinta reproducida. Mostrando mensaje.");
                MessageUI.Instance.Show(mensaje, duracionMensaje);
                mensajeMostrado = true;
            }
            else if (!HistoriaProgreso.cintaReproducida)
            {
                print("Cinta a�n no reproducida.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            print("Jugador cerca de la puerta.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}