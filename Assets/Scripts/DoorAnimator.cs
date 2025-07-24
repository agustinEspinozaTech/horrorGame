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

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < openDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (hasKey && !hasOpened)
            {
                animator.speed = 0.3f;
                animator.Play("Door_Open", 0, 0f);
                audioSource.Play();
                hasOpened = true;
            }
            else if (!hasKey)
            {
                MessageUI.Instance.Show("Parece que está cerrada con llave...\ncon suerte, deberia estar donde recuerdo.");
            }
        }
    }
}