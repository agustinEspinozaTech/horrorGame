using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyChaseImmediate : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [Header("Configuración de persecución")]
    [SerializeField] private float gameOverDistance = 1.5f;
    [SerializeField] private float chaseSpeed = 3.5f; // más rápido que el normal

    private NavMeshAgent agent;

    void Start()
    {
        // Buscar player si no está asignado
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = gameOverDistance;

        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (player != null && agent.isOnNavMesh)
            agent.SetDestination(player.position);

        // Si está lo suficientemente cerca => Game Over
        if (Vector3.Distance(transform.position, player.position) <= gameOverDistance)
        {
            SceneManager.LoadScene("Inicio");
        }
    }
}
