using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapePlayer : MonoBehaviour
{
    public Animator playerAnimator;
    public AudioSource audioSource;
    public DestructibleObject mesaDestructible;
    public float radius = 10f;
    public float power = 50f;

    [Tooltip("Duración real que quiero reproducir del audio y la vibración")]
    public float duracionPersonalizada = 14f;

    private bool playerInRange = false;
    private bool hasPlayed = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasPlayed)
        {
            hasPlayed = true; //  Se marca como ya reproducida
            HistoriaProgreso.cintaReproducida = true;
            StartCoroutine(PlayAndExplode());
        }
    }

    IEnumerator PlayAndExplode()
    {
        float magnitud = 0.2f;

        // BLOQUEA movimiento
        AudioController.bloqueadoPorAudio = true;

        // Activar animación de miedo
        if (playerAnimator != null)
            playerAnimator.SetBool("Terrified", true);

        // Vibración y sonido sincronizados
        CameraShaker.Instance.Shake(duracionPersonalizada, magnitud);
        audioSource.Play();
        yield return new WaitForSeconds(duracionPersonalizada);
        audioSource.Stop(); // corta el audio manualmente

       

        // Desactivar animación
        if (playerAnimator != null)
            playerAnimator.SetBool("Terrified", false);

        // DESBLOQUEA movimiento
        AudioController.bloqueadoPorAudio = false;

        // Explosión
        Vector3 explosionPos = mesaDestructible.transform.position;
        mesaDestructible.Break();

        // Mensaje después del audio
        MessageUI.Instance.Show("Esto fue puesto aquí para que lo escuchara...\nYa no importa nada, tengo que irme.", 4f);


        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.AddExplosionForce(power, explosionPos, radius, 1.0f);
            }
        }
     

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!hasPlayed)
                MessageUI.Instance.Show("¿Qué es eso?\nNo estaba ahí la última vez...", 5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public bool CintaYaReprodujo()
    {
        print("Consultando si la cinta fue reproducida: " + hasPlayed);
        return hasPlayed;

    }
}
