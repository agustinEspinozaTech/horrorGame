using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CartaInteractiva : MonoBehaviour
{
    [Header("Referencia al panel de la carta (Canvas)")]
    [SerializeField] private GameObject panelCarta;

    [Header("Objeto físico de la carta en la escena")]
    [SerializeField] private GameObject objetoCartaFisica;

    private bool cartaFueAbierta = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        // Detectar si en algún momento se abrió la carta
        if (panelCarta.activeSelf)
        {
            cartaFueAbierta = true;
        }

        // Si la carta fue abierta y ya se cerró, y aún no se mostró el mensaje
        if (cartaFueAbierta && !panelCarta.activeSelf && !mensajeMostrado)
        {
            MessageUI.Instance.Show("Presiona 'Q' para ocultar la carta");
            mensajeMostrado = true;
        }

        // Si ya mostramos el mensaje y el jugador presiona D
        if (mensajeMostrado && Input.GetKeyDown(KeyCode.Q))
        {
            MessageUI.Instance.Hide();

            if (objetoCartaFisica != null)
            {
                Destroy(objetoCartaFisica);

                // Guardar que la carta fue destruida
                HistoriaProgreso.cartaDestruida = true;
                print("Carta destruida y registrada en HistoriaProgreso.");
            }

            this.enabled = false;
        }
    }
}