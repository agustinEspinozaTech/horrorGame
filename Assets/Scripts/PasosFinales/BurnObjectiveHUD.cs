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

    [Header("Fuego")]
    [SerializeField] private GameObject fireObject; // fuego oculto en escena
    [SerializeField] private AudioSource fireSfx;   // sonido al encender fuego

    [Header("Config")]
    [SerializeField] private int requiredCount = 3;
    [SerializeField] private float startSeconds = 180f;
    [SerializeField] private bool autoStartOnSceneLoad = true;

    [Header("Aviso a 7 segundos")]
    [SerializeField] private AudioSource sfxSevenSeconds;
    [SerializeField] private bool playOneShot = true;

    [Header("Mensaje de quemar")]
    [SerializeField] private float burnPromptDelay = 2.2f;    // espera tras el último pickup
    [SerializeField] private float burnPromptDuration = 999f; // queda visible hasta que se quema

    // >>> ADDED: ubicación del fuego cerca del jugador
    [Header("Ubicación del fuego")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 fireOffset = new Vector3(0f, 0f, 1.2f);
    [SerializeField] private bool alignToGround = true;
    [SerializeField] private LayerMask groundMask;

    float secondsLeft;
    bool running;
    bool sevenCuePlayed;
    bool readyToBurn;

    void Start()
    {
        if (canvasRoot != null) canvasRoot.SetActive(false);
        if (fireObject != null) fireObject.SetActive(false);

        if (autoStartOnSceneLoad && HistoriaProgreso.hogueraObjetivoActivo)
        {
            StartObjective();
        }
    }

    void Update()
    {
        if (readyToBurn && Input.GetKeyDown(KeyCode.E))
        {
            // <<< ADDED: posicionar fuego cerca del jugador (y al piso si corresponde)
            if (fireObject != null && player != null)
            {
                Vector3 spawnPos = player.position
                    + player.forward * fireOffset.z
                    + player.right * fireOffset.x
                    + Vector3.up * fireOffset.y;

                if (alignToGround)
                {
                    Vector3 rayStart = spawnPos + Vector3.up * 2f;
                    if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 5f, groundMask))
                    {
                        spawnPos = hit.point;
                    }
                }

                fireObject.transform.position = spawnPos;

                Vector3 fwd = player.forward; fwd.y = 0f;
                if (fwd.sqrMagnitude > 0.0001f)
                    fireObject.transform.rotation = Quaternion.LookRotation(fwd);
            }

            // Activar fuego (tu lógica)
            if (fireObject != null) fireObject.SetActive(true);

            if (fireSfx != null)
            {
                if (fireSfx.clip != null) fireSfx.PlayOneShot(fireSfx.clip);
                else fireSfx.Play();
            }

            MessageUI.Instance?.Hide();
            readyToBurn = false;
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
        readyToBurn = false;

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
                ShowBurnMessage();
                break;
            }

            secondsLeft -= Time.deltaTime;

            if (!sevenCuePlayed && secondsLeft <= 7f)
            {
                if (sfxSevenSeconds != null)
                {
                    if (playOneShot && sfxSevenSeconds.clip != null)
                        sfxSevenSeconds.PlayOneShot(sfxSevenSeconds.clip);
                    else
                        sfxSevenSeconds.Play();
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
            ShowBurnMessage();
        }
    }

    void ShowBurnMessage()
    {
        StopCoroutine(nameof(ShowBurnMessageCo));
        StartCoroutine(ShowBurnMessageCo());
    }

    IEnumerator ShowBurnMessageCo()
    {
        yield return new WaitForSeconds(burnPromptDelay);

        if (MessageUI.Instance != null)
            MessageUI.Instance.Show("Presione el botón E para quemar los objetos", burnPromptDuration);

        readyToBurn = true;
    }
}
