using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepAudioPlayer : MonoBehaviour
{
    public AudioClip[] footstepClips;
    [Range(0f, 1f)] public float volume = 0.1f;
    public float stepInterval = 0.5f;

    private CharacterController controller;
    private float footstepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        footstepTimer -= Time.deltaTime;

        if (controller.isGrounded && controller.velocity.magnitude > 0.1f && footstepTimer <= 0f)
        {
            PlayFootstep();
            footstepTimer = stepInterval;
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, footstepClips.Length);

            AudioSource.PlayClipAtPoint(footstepClips[index], transform.position, volume);
        }
    }
}