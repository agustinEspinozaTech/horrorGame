using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public AudioSource idleAudioSource;
    public AudioController audioController;

    public float delayBeforeChase = 4f;
    public float chaseDuration = 8f;         //  Solo perseguirá por este tiempo
    public float gameOverDistance = 1.5f;
    public GameObject gameOverUI;

    private bool isChasing = false;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("isRunning", false);

        if (idleAudioSource != null)
            idleAudioSource.Play();

        if (audioController != null)
            audioController.TemporarilyLowerBackground(delayBeforeChase);

        StartCoroutine(StartChaseAfterDelay());
    }

    IEnumerator StartChaseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        if (idleAudioSource != null && idleAudioSource.isPlaying)
            idleAudioSource.Stop();

        //  Vibración de pantalla al empezar la persecución
        if (CameraShaker.Instance != null)
        {
            CameraShaker.Instance.Shake(1.2f, 0.5f); // duración, intensidad
        }

        animator.SetBool("isRunning", true);
        isChasing = true;

        StartCoroutine(StopChaseAfterSeconds(chaseDuration));
    }

    IEnumerator StopChaseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Si no te atrapó en ese tiempo, se destruye
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isChasing || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        agent.SetDestination(player.position);

        if (distance <= gameOverDistance && gameOverUI != null)
        {
            FindObjectOfType<GameOverManager>().activeEnemy = this.gameObject;
            FindObjectOfType<GameOverManager>().ShowGameOver();
        }
    }
}