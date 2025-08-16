using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    private AudioSource audioSource;

    public bool hasKey = false;

    private Animator animator;
    private bool hasOpened = false;

    public Transform player;
    public float openDistance = 3f;

    private bool isInRange;
    private Coroutine promptRoutine;
    private bool promptActive;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool nowInRange = distance < openDistance;

        if (nowInRange && !isInRange && !hasOpened)
        {
            isInRange = true;
            StartPrompt();
        }
        else if (!nowInRange && isInRange)
        {
            isInRange = false;
            CancelPrompt(true);
        }

        if (distance < openDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (hasKey && !hasOpened)
            {
                CancelPrompt(true);
                animator.speed = 0.3f;
                animator.Play("Door_Open", 0, 0f);
                audioSource.Play();
                hasOpened = true;
            }
            else if (!hasKey)
            {
                CancelPrompt(false);
                MessageUI.Instance.Show("Parece que está cerrada con llave...\ncon suerte, deberia estar donde recuerdo.");
            }
        }
    }

    void StartPrompt()
    {
        CancelPrompt(false);
        promptRoutine = StartCoroutine(ShowPromptForSeconds(3f));
    }

    void CancelPrompt(bool hideNow)
    {
        if (promptRoutine != null)
        {
            StopCoroutine(promptRoutine);
            promptRoutine = null;
        }
        if (promptActive && hideNow)
        {
            MessageUI.Instance.Hide();
        }
        promptActive = false;
    }

    IEnumerator ShowPromptForSeconds(float seconds)
    {
        promptActive = true;
        MessageUI.Instance.Show("Presione E para intentar abrir la puerta");
        yield return new WaitForSeconds(seconds);
        if (promptActive)
        {
            MessageUI.Instance.Hide();
            promptActive = false;
        }
        promptRoutine = null;
    }
}
