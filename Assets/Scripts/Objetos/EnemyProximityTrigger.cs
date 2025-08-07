using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyProximityTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource idleAudioSource;

    [Header("Configuración de persecución")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float chaseDuration = 10f;
    [SerializeField] private float gameOverDistance = 1.5f;

  //  private bool isChasing = false;
    private bool hasTriggered = false;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        animator.SetBool("isRunning", false);

        if (idleAudioSource != null)
            idleAudioSource.Play();
    }

    void Update()
    {
        if (hasTriggered) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            hasTriggered = true;
            StartChase();
        }
    }

    private void StartChase()
    {
        if (idleAudioSource != null && idleAudioSource.isPlaying)
            idleAudioSource.Stop();

        agent.enabled = true;

        animator.SetBool("isRunning", true);
       // isChasing = true;

        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        float elapsed = 0f;

        while (elapsed < chaseDuration)
        {
            if (player != null && agent.isOnNavMesh)
                agent.SetDestination(player.position);

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= gameOverDistance)
            {
                SceneManager.LoadScene("Inicio"); // cambio de escena
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
