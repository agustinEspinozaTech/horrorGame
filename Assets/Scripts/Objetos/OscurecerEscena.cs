using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscurecerEscena : MonoBehaviour
{
    [Header("Configuración de oscuridad")]
    [SerializeField] private float intensidadInicial = 1.5f;
    [SerializeField] private float intensidadMinima = 0.2f;

    private int totalEventos;
    private int eventosActivados;

    void Start()
    {
        totalEventos = 3; // cinta y carta
        RenderSettings.ambientIntensity = intensidadInicial;
    }
    void Update()
    {
        eventosActivados = 0;

        if (HistoriaProgreso.cintaReproducida) eventosActivados++;
        if (HistoriaProgreso.cartaDestruida) eventosActivados++;
        if (HistoriaProgreso.fotografiaDestruida) eventosActivados++;

        float porcentajeOscuridad = (float)eventosActivados / totalEventos;
        float nuevaIntensidad = Mathf.Lerp(intensidadInicial, intensidadMinima, porcentajeOscuridad);
        RenderSettings.ambientIntensity = nuevaIntensidad;
    }
}