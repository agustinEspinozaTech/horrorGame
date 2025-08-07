using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FotografiaInteractiva : MonoBehaviour
{
    [Header("UI de la fotografía")]
    [SerializeField] private GameObject panelFotografia;

    [Header("Cámara del jugador")]
    [SerializeField] private MonoBehaviour scriptMovimientoCamara;

    [Header("Objeto físico en la escena")]
    [SerializeField] private GameObject objetoFotografiaFisica;

    private bool jugadorCerca = false;
    private bool fotografiaFueAbierta = false;
    private bool mensajeMostrado = false;

    void Update()
    {
        if (jugadorCerca && !panelFotografia.activeSelf && !mensajeMostrado)
        {
            MessageUI.Instance.Show("Presiona 'E' para mirar la fotografía");
            mensajeMostrado = true;
        }

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            AbrirFotografia();
        }

        if (panelFotografia.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarFotografia();
        }

        if (panelFotografia.activeSelf)
        {
            fotografiaFueAbierta = true;
        }

        if (fotografiaFueAbierta && !panelFotografia.activeSelf && !mensajeMostrado)
        {
            MessageUI.Instance.Show("Presiona 'D' para dejar la fotografía");
            mensajeMostrado = true;
        }

        if (mensajeMostrado && Input.GetKeyDown(KeyCode.D))
        {
            MessageUI.Instance.Hide();
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            scriptMovimientoCamara.enabled = true;

            if (objetoFotografiaFisica != null)
            {
                Destroy(objetoFotografiaFisica);
                HistoriaProgreso.fotografiaDestruida = true;
                print("Fotografía destruida y registrada en HistoriaProgreso.");
            }

            this.enabled = false;
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
            MessageUI.Instance.Hide();
        }
    }

    public void AbrirFotografia()
    {
        panelFotografia.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        scriptMovimientoCamara.enabled = false;
        MessageUI.Instance.Hide();
    }

    public void CerrarFotografia()
    {
        panelFotografia.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        scriptMovimientoCamara.enabled = true;
        fotografiaFueAbierta = true;
        mensajeMostrado = false;
    }
}
