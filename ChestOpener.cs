using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    public Animator animator;
    public Animator victoryAnimator;
    public PlayerController playerController;
    private bool opened=false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && opened==false)
        {
            Debug.Log("Player collided with chest");

            animator.SetTrigger("open");
            playerController.EndOfGame();
            victoryAnimator.SetTrigger("pop");
            opened = true;
        }
    }
}
