using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
            animator.SetFloat("Speed", 0.11f);
        else
            animator.SetFloat("Speed", 0);
    }
}
