using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuertaPorHistoria : MonoBehaviour
{
    private bool yaOcultada = false;

    void Update()
    {
        if (HistoriaProgreso.cintaReproducida && !yaOcultada)
        {
            OcultarPuerta();
            yaOcultada = true;
        }
    }

    private void OcultarPuerta()
    {
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
