using UnityEngine;

public static class HistoriaProgreso
{
    public static bool cintaReproducida = false;
    public static bool cartaDestruida = false;
    public static bool fotografiaDestruida = false;

    public static bool narrativaFinalMostrada = false;

    // Retorno
    public static bool hasReturnPoint = false;
    public static Vector3 returnPos;
    public static Vector3 returnEuler;

    // Ritual
    public static bool reaccionTestigosMostrada = false;

    public static bool ritualActivado = false;    // ya se activó el objetivo del ritual
    public static bool libroRecogido = false;
    public static bool cruzRecogida = false;
    public static bool velaRecogida = false;
    public static bool puertaSalidaDesactivada = false; // la puerta ya fue "abierta" (desactivada)

    // Enemigo
    public static bool enemigoDebePersistir = false; // el enemigo debe estar activo al volver
}
