using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public Transform player;
    public float pickupDistance = 2.5f;
    public DoorAnimator doorScript;

    public float delayToDisappear = 0.5f;

    private bool isCollected = false;

    public bool HasBeenCollected()
    {
        return isCollected;
    }

    void Update()
    {
        if (isCollected) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < pickupDistance && Input.GetKeyDown(KeyCode.E))
        {
            isCollected = true;
            doorScript.hasKey = true;

            StartCoroutine(OcultarLlaveConDelay());

            MessageUI.Instance.Show("La encontré… \naunque parte de mí preferiría no haberlo hecho.");
        }
    }

    private IEnumerator OcultarLlaveConDelay()
    {
        yield return new WaitForSeconds(delayToDisappear);
        gameObject.SetActive(false);
    }
}