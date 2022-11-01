using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovementController : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator.SetFloat("speed", 1f);
    }

    private void Update()
    {
        if (Toolbox.GameplayScript.useSlowPowerup)
            animator.SetFloat("speed", 0.15f);
        else
            animator.SetFloat("speed", 1f);
    }
}
