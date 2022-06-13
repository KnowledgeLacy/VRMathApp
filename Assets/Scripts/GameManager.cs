using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public GameObject questionText;
    public GameObject scoreText;

    float scoreCount;
    float totalQuestions;

    public string userId = "User 1";
    public float scorePercentage;
    public string jsonData;

    public UserScore UserIdAndScore = new UserScore();
    public ServerlessAPIController ServerlessAPIController;

    public GameObject menu;
    public GameObject correctOrWrongCanvas;
    //public GameObject gameStartCanvas;
    public GameObject quizQuestionsCanvas;
    public GameObject gameEndCanvas;
    public GameObject scoreSavedCanvas;
    public GameObject scoreListCanvas;

    public GameObject SaveButton;
    public GameObject LoadButton;

    

    public void Start()
    {
        totalQuestions = QnA.Count;
        scoreCount = 0f;
        correctOrWrongCanvas.GetComponent<TMP_Text>().text = "";
        gameEndCanvas.SetActive(false);
        menu.SetActive(false);
        scoreSavedCanvas.SetActive(false);
        scoreListCanvas.SetActive(false);
        quizQuestionsCanvas.SetActive(true);
        
        GenerateQuestion();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        correctOrWrongCanvas.GetComponent<TMP_Text>().text = "";
    }

    public void SaveScore()
    {
        ServerlessAPIController.OnButtonSaveScore();
        
        scoreListCanvas.SetActive(false);
        quizQuestionsCanvas.SetActive(false);
        gameEndCanvas.SetActive(false);
        scoreSavedCanvas.SetActive(true);

    }

    public void LoadScores()
    {
        ServerlessAPIController.OnButtonLoadScores();

        scoreSavedCanvas.SetActive(false);
        quizQuestionsCanvas.SetActive(false);
        gameEndCanvas.SetActive(false);
        scoreListCanvas.SetActive(true);
    }

    void GameOver()
    {
        quizQuestionsCanvas.SetActive(false);
        gameEndCanvas.SetActive(true);
        menu.SetActive(true);

        scorePercentage = (scoreCount / totalQuestions) * 100;

        UserIdAndScore.userId = userId;
        UserIdAndScore.score = scorePercentage.ToString();
        jsonData = JsonUtility.ToJson(UserIdAndScore);
        
        scoreText.GetComponent<TMP_Text>().text = scoreCount + "/" + totalQuestions + " (" + scorePercentage + "%)";

    }


    public void Correct()
    {
        scoreCount += 1f;
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            if (QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        if (QnA.Count > 0)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            currentQuestion = Random.Range(0, QnA.Count);
            questionText.GetComponent<TMP_Text>().text = QnA[currentQuestion].Question;
        }
        else
        {
            GameOver();
        }

        SetAnswer();
    }
}
