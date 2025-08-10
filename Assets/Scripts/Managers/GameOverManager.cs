using System.Collections;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Refs")]
    public GameObject activeEnemy;                          //  público para EnemyAI
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Checkpoint")]
    [SerializeField] private string checkpointTag = "Checkpoint";

    [Header("Game Over")]
    [SerializeField] private int countdownSeconds = 5;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = false;

    Vector3 checkpointPosition;
    bool checkpointFound;
    bool gameOverActive;
    Coroutine countdownCo;

    void Start()
    {
        if (gameOverUI) gameOverUI.SetActive(false);

        var checkpoint = GameObject.FindGameObjectWithTag(checkpointTag);
        if (checkpoint)
        {
            checkpointPosition = checkpoint.transform.position;
            checkpointFound = true;
        }

        if (verboseLogs) print("[GameOver] Start. CheckpointFound=" + checkpointFound);
    }

    public void ShowGameOver()
    {
        if (gameOverActive) return;          // idempotente
        gameOverActive = true;

        Time.timeScale = 0f;
        if (gameOverUI) gameOverUI.SetActive(true);

        if (countdownCo != null) StopCoroutine(countdownCo);
        countdownCo = StartCoroutine(CountdownToRetry());

        if (verboseLogs) print("[GameOver] Activado");
    }

    IEnumerator CountdownToRetry()
    {
        int count = Mathf.Max(1, countdownSeconds);

        while (count > 0)
        {
            if (countdownText) countdownText.text = $"Reiniciando en {count}...";
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        if (countdownText) countdownText.text = "";
        Retry();
    }

    void Retry()
    {
        Time.timeScale = 1f;

        if (player && checkpointFound) player.position = checkpointPosition;
        if (activeEnemy) Destroy(activeEnemy);
        if (gameOverUI) gameOverUI.SetActive(false);
        if (countdownText) countdownText.text = "";

        gameOverActive = false;
        countdownCo = null;

        if (verboseLogs) print("[GameOver] Retry completado");
    }
}
