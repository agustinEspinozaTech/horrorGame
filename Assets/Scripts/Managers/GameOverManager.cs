using System.Collections;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject activeEnemy;
    public GameObject gameOverUI;
    public Transform player;
    public TextMeshProUGUI countdownText;
    public string checkpointTag = "Checkpoint";

    private Vector3 checkpointPosition;

    void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        GameObject checkpoint = GameObject.FindGameObjectWithTag(checkpointTag);
        if (checkpoint != null)
            checkpointPosition = checkpoint.transform.position;
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        StartCoroutine(CountdownToRetry());
    }

    IEnumerator CountdownToRetry()
    {
        int count = 5;

        while (count > 0)
        {
            countdownText.text = $"Reiniciando en {count}...";
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        countdownText.text = ""; // Limpiar el texto
        Retry();
    }

    void Retry()
    {
        Time.timeScale = 1f;

        if (player != null)
            player.position = checkpointPosition;

        if (activeEnemy != null)
            Destroy(activeEnemy);

        gameOverUI.SetActive(false);
        countdownText.text = ""; //  Limpiar el texto por seguridad
    }
}
