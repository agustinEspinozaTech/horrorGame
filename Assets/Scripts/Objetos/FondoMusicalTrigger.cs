using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMusicalTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource musicaFondo;
    [SerializeField] private AudioSource gritoMostruo;

    private bool activado = false;

    void Update()
    {
        if (activado) return;

        if (HistoriaProgreso.cintaReproducida &&
            HistoriaProgreso.cartaDestruida &&
            HistoriaProgreso.fotografiaDestruida)
        {
            activado = true;
            StartCoroutine(EsperarYReproducirMusica());
        }
    }

    IEnumerator EsperarYReproducirMusica()
    {
        if (gritoMostruo != null && gritoMostruo.isPlaying)
        {
            print("Esperando que termine el grito...");
            yield return new WaitWhile(() => gritoMostruo.isPlaying);
        }

        if (musicaFondo != null)
        {
            musicaFondo.Play();
            print("Música de fondo iniciada.");
        }
    }
}