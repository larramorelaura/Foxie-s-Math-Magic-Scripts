using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManipulativePopManager : MonoBehaviour
{
    // public TMP_Text problemtext;
    public PopUpSystem popupSystem;
    public Animator animator;
    

    public void DisplayManipulativePanel(TMP_Text problemText)
    {
        // animator.SetTrigger("pop");
        string prob = problemText.text;
        Debug.Log("Problem text: " + prob);
        popupSystem.PopUp(prob);
    }
}
