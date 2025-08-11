using UnityEngine;
using TMPro;
using System.Collections;

public class BurnObjectiveHUD : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject canvasRoot;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private HordeActivator hordeActivator;

    [Header("Config")]
    [SerializeField] private int requiredCount = 3;
    [SerializeField] private float startSeconds = 180f;
    [SerializeField] private bool autoStartOnSceneLoad = true;

    [Header("Aviso a 7 segundos")]
    [SerializeField] private AudioSource sfxSevenSeconds;   // asigna un AudioSource con el clip
    [SerializeField] private bool playOneShot = true;

    float secondsLeft;
    bool running;
    bool sevenCuePlayed;   //  para no repetir el sonido

    void Start()
    {
        if (canvasRoot != null) canvasRoot.SetActive(false);

        if (autoStartOnSceneLoad && HistoriaProgreso.hogueraObjetivoActivo)
        {
            StartObjective();
        }
    }

    public void StartObjective()
    {
        if (!HistoriaProgreso.hogueraObjetivoActivo)
            HistoriaProgreso.hogueraObjetivoActivo = true;

        if (Time.timeScale == 0f) Time.timeScale = 1f;

        secondsLeft = Mathf.Max(0f, startSeconds);
        running = true;
        sevenCuePlayed = false;

        if (canvasRoot != null) canvasRoot.SetActive(true);

        UpdateCounterUI(HistoriaProgreso.hogueraObjetosRecogidos);
        UpdateTimeUI(secondsLeft);

        StopAllCoroutines();
        StartCoroutine(TimerCo());
    }

    IEnumerator TimerCo()
    {
        while (running)
        {
            if (HistoriaProgreso.hogueraObjetosRecogidos >= requiredCount)
            {
                running = false;
                break;
            }

            secondsLeft -= Time.deltaTime;

            // Sonido exactamente cuando queden 7 segundos (una sola vez)
            if (!sevenCuePlayed && secondsLeft <= 7f)
            {
                if (sfxSevenSeconds != null)
                {
                    if (playOneShot && sfxSevenSeconds.clip != null) sfxSevenSeconds.PlayOneShot(sfxSevenSeconds.clip);
                    else sfxSevenSeconds.Play();
                }
                sevenCuePlayed = true;
            }

            if (secondsLeft <= 0f)
            {
                secondsLeft = 0f;
                UpdateTimeUI(secondsLeft);
                running = false;

                if (hordeActivator != null) hordeActivator.ActivateHorde();
                break;
            }

            UpdateTimeUI(secondsLeft);
            yield return null;
        }
    }

    void UpdateTimeUI(float seconds)
    {
        if (!countdownText) return;
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        countdownText.text = $"{m:00}:{s:00}";
    }

    void UpdateCounterUI(int current)
    {
        if (!counterText) return;
        counterText.text = $"Objetos: {current}/{requiredCount}";
    }

    public void OnBurnItemCollected()
    {
        if (HistoriaProgreso.hogueraObjetosRecogidos < requiredCount)
            HistoriaProgreso.hogueraObjetosRecogidos++;

        UpdateCounterUI(HistoriaProgreso.hogueraObjetosRecogidos);

        if (HistoriaProgreso.hogueraObjetosRecogidos >= requiredCount)
        {
            running = false;
            // if (canvasRoot) canvasRoot.SetActive(false);
        }
    }
}
