using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitnessSequenceController : MonoBehaviour
{
    [SerializeField] private bool startWhenThreeFlagsTrue = true;
    [SerializeField] private float firstDelaySeconds = 5f;
    [SerializeField] private float appearDurationSeconds = 40f;
    [SerializeField] private bool hideAfterAppearance = true;
    [SerializeField] private List<GameObject> witnessesInScene = new List<GameObject>();
    [SerializeField] private Transform lookAtTarget;

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

        // Aparecen todos a la vez
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

        if (hideAfterAppearance)
        {
            yield return new WaitForSeconds(appearDurationSeconds);

            for (int i = 0; i < witnessesInScene.Count; i++)
                if (witnessesInScene[i] != null) witnessesInScene[i].SetActive(false);
        }
    }
}
