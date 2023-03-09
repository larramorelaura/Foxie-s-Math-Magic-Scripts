using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathProblem : MonoBehaviour
{
    private int firstNumber;
    private int secondNumber;
    private int answer;
    public PopUpSystem popUpSystem;
    public TMP_InputField answerInput;
    public TMP_Text problemCorrectText;
    public TMP_Text jewelText;
    public TMP_Text coinText;
    public Animator animator;
    public Animator animator2;
    public PlayerController playerController;
    int coins=0;
    int jewels=0;

    public void DisplayProblem()
    {
        
        // Generate random math problem and display it to player
        firstNumber = Random.Range(0, 15);
        secondNumber = Random.Range(0, 15);
        answer = firstNumber * secondNumber;
        Debug.Log("What is " + firstNumber + " x " + secondNumber + "?");
        string problemText = "What is " + firstNumber + " x " + secondNumber + "?";
        popUpSystem.PopUp(problemText);
        answerInput.text = "";
    }

    public bool CheckAnswer()
    {
        int playerAnswer;
        bool isNumeric = int.TryParse(answerInput.text, out playerAnswer);

        if (isNumeric && playerAnswer == answer)
        {
            // Do something if answer is correct
            Debug.Log("success");
            animator.SetTrigger("pop");
            animator2.SetTrigger("close");
            playerController.ResumeMovement();
            if(firstNumber==0 || firstNumber==1 || secondNumber==0 ||secondNumber==1)
            {
                coins=8;
                jewels=2;
            }
            else
            {
            coins = firstNumber*2;
            jewels =secondNumber;
            }
            playerController.AddScore(coins, jewels);
            problemCorrectText.text= firstNumber + " x " + secondNumber + " = " + answer;
            jewelText.text=jewels.ToString();
            coinText.text=coins.ToString();
            return true;
        }
        else
        {
            Debug.Log("wrong");
            return false;
        }
    }
    public void CheckAnswerAndDoAction()
    {
        if (CheckAnswer())
        {
            Debug.Log("checked");
        }
        else
        {
            Debug.Log("checked and wrong");
        }
    }

}

