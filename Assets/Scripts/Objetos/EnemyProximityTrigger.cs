using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro; // <<< ADDED (si usas TMP)

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

    // <<< ADDED: UI Game Over
    [Header("Game Over UI")]
    [SerializeField] private GameObject canvasGameOver;           // Canvas a mostrar
    [SerializeField] private TextMeshProUGUI countdownText;       // Texto del contador (TMP)
    [SerializeField] private float countdownSeconds = 3f;         // Segundos de cuenta regresiva

    private bool hasTriggered = false;
    private bool gameOverStarted = false; // <<< ADDED: evita dobles disparos
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        animator.SetBool("isRunning", false);
        if (idleAudioSource != null) idleAudioSource.Play();

        // <<< ADDED: aseguramos que el canvas esté oculto de inicio
        if (canvasGameOver != null) canvasGameOver.SetActive(false);
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
                // <<<< REEMPLAZO MINIMO: en vez de cargar escena al toque,
                // mostramos canvas + cuenta regresiva y AL FINAL hacemos lo mismo que ya hacías
                if (!gameOverStarted)
                {
                    gameOverStarted = true;
                    // opcional: frenar enemigo durante la cuenta (no altera tu lógica de reset/escena)
                    if (agent != null) agent.isStopped = true;
                    animator.SetBool("isRunning", false);

                    yield return StartCoroutine(ShowGameOverAndRestart()); // <<< ADDED
                }
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // <<< ADDED: muestra el canvas, cuenta y luego ejecuta tu mismo bloque original
    IEnumerator ShowGameOverAndRestart()
    {
        if (canvasGameOver != null) canvasGameOver.SetActive(true);

        float timeLeft = Mathf.Max(1f, countdownSeconds);
        // Usamos tiempo real por si en algún momento estás manipulando timeScale en otro lado
        float endTime = Time.realtimeSinceStartup + timeLeft;

        while (Time.realtimeSinceStartup < endTime)
        {
            float rest = Mathf.Ceil(endTime - Time.realtimeSinceStartup);
            if (countdownText != null)
            {
                countdownText.text = rest.ToString("0"); // 3, 2, 1...
            }
            yield return null;
        }

        // --- AQUÍ EJECUTAMOS EXACTAMENTE TU LÓGICA ACTUAL ---
        Time.timeScale = 1f;
        AudioController.bloqueadoPorAudio = false;
        MessageUI.Instance?.Hide();
        HistoriaProgreso.ResetAll();

        SceneManager.LoadScene("DestruirEvidencia");
    }
}
