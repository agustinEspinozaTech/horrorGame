using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcultarPuertaPorCarta : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoPuerta; // Asigná el audio en el Inspector

    private bool yaOcultada = false;

    void Update()
    {
        if (HistoriaProgreso.cartaDestruida && !yaOcultada)
        {
            OcultarPuerta();
            yaOcultada = true;
        }
    }

    private void OcultarPuerta()
    {
        if (sonidoPuerta != null)
        {
            sonidoPuerta.Play();
        }

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }
}
