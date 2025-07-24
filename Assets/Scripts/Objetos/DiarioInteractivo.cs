using System.Collections;
using UnityEngine;

public class DiarioInteractivo : MonoBehaviour
{
    [Header("UI del Diario")]
    public GameObject panelUI;

    private bool jugadorCerca = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            UnityEngine.Debug.Log("Presionaste E dentro del trigger");
            panelUI.SetActive(true);
            Time.timeScale = 0f;

            //  Mostrar y desbloquear cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (panelUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarDiario();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Algo entró: " + other.name);
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Entró el jugador: " + other.name);
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }

    public void CerrarDiario()
    {
        panelUI.SetActive(false);
        Time.timeScale = 1f;

        //  Ocultar y bloquear cursor de nuevo (modo juego)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
