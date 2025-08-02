using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CartaInteractiva : MonoBehaviour
{
    [Header("Referencia al panel de la carta (Canvas)")]
    [SerializeField] private GameObject panelCarta;

    [Header("Objeto f�sico de la carta en la escena")]
    [SerializeField] private GameObject objetoCartaFisica;

    private bool cartaFueAbierta = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        // Detectar si en alg�n momento se abri� la carta
        if (panelCarta.activeSelf)
        {
            cartaFueAbierta = true;
        }

        // Si la carta fue abierta y ya se cerr�, y a�n no se mostr� el mensaje
        if (cartaFueAbierta && !panelCarta.activeSelf && !mensajeMostrado)
        {
            MessageUI.Instance.Show("Presiona 'D' para ocultar la carta");
            mensajeMostrado = true;
        }

        // Si ya mostramos el mensaje y el jugador presiona D
        if (mensajeMostrado && Input.GetKeyDown(KeyCode.D))
        {
            MessageUI.Instance.Hide();

            if (objetoCartaFisica != null)
            {
                Destroy(objetoCartaFisica);
            }
  

            this.enabled = false;
        }
    }
}