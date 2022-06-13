using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject correctOrWrongCanvas;
    public bool isCorrect = false;

    public void Answer()
    {
        if (isCorrect)
        {
            correctOrWrongCanvas.GetComponent<TMP_Text>().text = "Correct";
            //play correct audio
            gameManager.Correct();
        }
        else
        {
            correctOrWrongCanvas.GetComponent<TMP_Text>().text = "Wrong";
            //play wrong audio
            gameManager.Wrong();
        }
    }
 
}
