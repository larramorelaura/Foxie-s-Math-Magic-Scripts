using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public PopUpSystem popUpSystem;
    public string popUpMessage;
    public Animator animator;
    public bool notInteracted=true;
    public bool freezePlayer=false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && notInteracted)
        {
            Debug.Log("Player collided with character");
            animator.SetTrigger("idle");
            popUpSystem.PopUp(popUpMessage);
            notInteracted=false;
            
        }
    }
}
