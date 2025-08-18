using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyChaseImmediate : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [Header("Configuración de persecución")]
    [SerializeField] private float gameOverDistance = 1.5f;
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Game Over UI")]
    [SerializeField] private GameObject canvasGameOver;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownSeconds = 3f;

    private NavMeshAgent agent;
    private bool gameOverStarted;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = gameOverDistance;

        if (animator != null) animator.SetBool("isRunning", true);
        if (canvasGameOver != null) canvasGameOver.SetActive(false);

        print("EnemyChaseImmediate: iniciado");
    }

    void Update()
    {
        if (player != null && agent.isOnNavMesh)
            agent.SetDestination(player.position);

        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= gameOverDistance)
        {
            if (!gameOverStarted)
            {
                gameOverStarted = true;
                print("EnemyChaseImmediate: Game Over detectado, iniciando cuenta regresiva");

                if (agent != null) agent.isStopped = true;
                if (animator != null) animator.SetBool("isRunning", false);

                StartCoroutine(ShowGameOverAndRestart());
            }
        }
    }

    System.Collections.IEnumerator ShowGameOverAndRestart()
    {
        if (canvasGameOver != null) canvasGameOver.SetActive(true);

        float timeLeft = Mathf.Max(1f, countdownSeconds);
        float endTime = Time.realtimeSinceStartup + timeLeft;

        while (Time.realtimeSinceStartup < endTime)
        {
            float rest = Mathf.Ceil(endTime - Time.realtimeSinceStartup);
            if (countdownText != null) countdownText.text = rest.ToString("0");
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("DestruirEvidencia");
    }
}
