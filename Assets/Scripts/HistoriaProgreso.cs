using UnityEngine;

public static class HistoriaProgreso
{
    // Evidencias iniciales
    public static bool cintaReproducida = false;
    public static bool cartaDestruida = false;
    public static bool fotografiaDestruida = false;

    // Estado de escenas / triggers
    public static bool narrativaFinalMostrada = false;
    public static bool reaccionTestigosMostrada = false;

    // Punto de retorno
    public static bool hasReturnPoint = false;
    public static Vector3 returnPos;
    public static Vector3 returnEuler;

    // Ritual intermedio (libro / cruz / vela)
    public static bool ritualActivado = false;
    public static bool libroRecogido = false;  //  usado por RitualItemPickup / Restorer
    public static bool cruzRecogida = false;  // 
    public static bool velaRecogida = false;  // 
    public static bool puertaSalidaDesactivada = false; //  usado por Restorer / ObjectiveManager
    public static bool enemigoDebePersistir = false;

    // Objetivo final: preparar hoguera (si ya lo estás usando)
    public static bool hogueraObjetivoActivo = false;
    public static int hogueraObjetosRecogidos = 0; // 0..3
    public static bool hogueraMadera = false;
    public static bool hogueraItemB = false;
    public static bool hogueraItemC = false;
}
