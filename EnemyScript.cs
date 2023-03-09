using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Animator animator;
    public Animator playerAnimator;

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         if (other.transform.position.y > transform.position.y)
    //         {
    //             // Player jumped on enemy
    //             animator.SetTrigger("destroy");
    //             Destroy(gameObject); // Kill the enemy
    //         }
    //         else
            
    //             // Player ran into enemy
    //             if (playerController != null)
    //             {
    //                 playerController.TakeDamage(10); // Reduce player's health
    //                 playerAnimator.SetTrigger("damage");
    //             }
            
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Check if the player collided with the enemy from above
                float playerBottomY = other.transform.position.y - (other.bounds.size.y / 2);
                float enemyTopY = transform.position.y + (GetComponent<BoxCollider2D>().size.y / 2);
                if (playerBottomY >= enemyTopY)
                {
                    // Player jumped on enemy
                    animator.SetTrigger("destroy");
                    Destroy(gameObject); // Kill the enemy
                    playerController.Jump(4.0f);
                }
                else
                {
                    // Player ran into enemy
                    playerController.TakeDamage(10); // Reduce player's health
                    playerAnimator.SetTrigger("damage");
                }
            }
        }
    }


}
