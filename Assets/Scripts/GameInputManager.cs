using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class GameInputManager : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // Salir del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SalirDelJuego();
        }

        // Pausar o reanudar
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePausa();
        }
    }

    void SalirDelJuego()
    {
       print("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
#else
        Application.Quit(); // Cierra el ejecutable
#endif
    }

    void TogglePausa()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        print(isPaused ? "Juego en pausa" : "Juego reanudado");
    }
}
