using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    //Used by enemy to find Target

    private Animator animator;
    public GameObject target;


    private void Start()
    {
        GameManager.Instance.OnOutOfLives.AddListener(HandleOutOfLives);
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);
    }

    private void HandleOutOfLives(bool isOutOfLives)
    {
        if (isOutOfLives)
        {
            animator.SetBool("isRunning", false);
            FindObjectOfType<AudioManager>().Play("Cough");
        }
    }
}
