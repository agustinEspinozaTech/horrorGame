using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrifiedKeyTest : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool current = animator.GetBool("Terrified");
            animator.SetBool("Terrified", !current); // alterna true/false
        }
    }
}