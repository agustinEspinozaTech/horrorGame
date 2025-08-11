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
    [SerializeField] private float chaseDuration = 5f;   // 5s
    [SerializeField] private float gameOverDistance = 1.5f;
    [SerializeField] private float chaseSpeed = 2.0f;    // más lento

    private bool hasTriggered = false;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        animator.SetBool("isRunning", false);
        if (idleAudioSource != null) idleAudioSource.Play();
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

    void StartChase()
    {
        if (idleAudioSource != null && idleAudioSource.isPlaying) idleAudioSource.Stop();

        agent.enabled = true;
        agent.speed = chaseSpeed;                //  velocidad más baja
        agent.stoppingDistance = gameOverDistance;

        animator.SetBool("isRunning", true);

        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        float elapsed = 0f;

        while (elapsed < chaseDuration)
        {
            if (player != null && agent.isOnNavMesh)
                agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= gameOverDistance)
            {
                SceneManager.LoadScene("Inicio");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
