using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitnessSequenceController : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private bool startWhenThreeFlagsTrue = true;
    [SerializeField] private float firstDelaySeconds = 5f;

    [Header("Visibilidad de testigos")]
    [SerializeField] private float appearDurationSeconds = 20f;
    [SerializeField] private bool hideAfterAppearance = true;
    [SerializeField] private List<GameObject> witnessesInScene = new List<GameObject>();
    [SerializeField] private Transform lookAtTarget;

    [Header("Reacción del protagonista")]
    [SerializeField, TextArea(2, 6)]
    private List<string> reactionLines = new List<string>
    {
        "No…\n no puede ser. Ya fue suficiente",
        "Esto se salió de control. No debo quedarme.",
        "Si me quedo, no salgo vivo de esta casa.",
        "Me voy ahora mismo. Ya."
    };
    [SerializeField] private float lineDuration = 3f;
    [SerializeField] private float timeBetweenLines = 0.6f; // pequeño respiro entre frases

    bool sequenceStarted;

    void Awake()
    {
        for (int i = 0; i < witnessesInScene.Count; i++)
            if (witnessesInScene[i] != null) witnessesInScene[i].SetActive(false);
    }

    void OnEnable()
    {
        if (!sequenceStarted) StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        if (startWhenThreeFlagsTrue)
        {
            yield return new WaitUntil(() =>
                HistoriaProgreso.cintaReproducida &&
                HistoriaProgreso.cartaDestruida &&
                HistoriaProgreso.fotografiaDestruida);
        }

        sequenceStarted = true;
        yield return new WaitForSeconds(firstDelaySeconds);

        // 1) Aparecen todos
        for (int i = 0; i < witnessesInScene.Count; i++)
        {
            var go = witnessesInScene[i];
            if (go == null) continue;

            go.SetActive(true);

            if (lookAtTarget != null)
            {
                var t = go.transform;
                var dir = lookAtTarget.position - t.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.001f)
                    t.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
        }

        // 2) Reacción del protagonista (en secuencia)
        StartCoroutine(PlayReactionLines());

        // 3) Ocultar si corresponde
        if (hideAfterAppearance)
        {
            yield return new WaitForSeconds(appearDurationSeconds);
            for (int i = 0; i < witnessesInScene.Count; i++)
                if (witnessesInScene[i] != null) witnessesInScene[i].SetActive(false);
        }
    }

    IEnumerator PlayReactionLines()
    {
        // Si ya se mostró antes, no volver a mostrarla
        if (HistoriaProgreso.reaccionTestigosMostrada) yield break;

        if (reactionLines == null || reactionLines.Count == 0) yield break;

        HistoriaProgreso.reaccionTestigosMostrada = true; // marcar como mostrada

        for (int i = 0; i < reactionLines.Count; i++)
        {
            MessageUI.Instance.Show(reactionLines[i], lineDuration);
            yield return new WaitForSeconds(lineDuration + timeBetweenLines);
        }
    }
}
