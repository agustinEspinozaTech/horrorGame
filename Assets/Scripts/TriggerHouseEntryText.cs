using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHouseEntryText : MonoBehaviour
{
    public GameObject textSequenceObject; 

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            textSequenceObject.SetActive(true);
        }
    }
}
