using System.Collections;
using UnityEngine;

public class DiarioInteractivo : MonoBehaviour
{
    [Header("UI del Diario")]
    public GameObject panelUI;

    private bool jugadorCerca = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        if (jugadorCerca && !panelUI.activeSelf && !mensajeMostrado)
        {
            MessageUI.Instance.Show("Presiona 'E' para abrir el diario");
            mensajeMostrado = true;
        }

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            panelUI.SetActive(true);
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            MessageUI.Instance.Hide(); // Ocultar mensaje al abrir diario
        }

        if (panelUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarDiario();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            mensajeMostrado = false;
            MessageUI.Instance.Hide(); // Ocultar mensaje al alejarse
        }
    }

    public void CerrarDiario()
    {
        panelUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
