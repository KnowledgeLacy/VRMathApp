using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class ServerlessAPIController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject scoreListUserIdsParent;
    public GameObject scoreListScoresParent;
    public GameObject scoreSavedText;
    public GameObject[] userIdPrefabs;
    public GameObject[] scorePrefabs;
    public string[] userIds;
    public string[] scoreList;

    //public List<GameObject> scoreListUserIds = new List<GameObject>();
    //public List<GameObject> scoreListScores = new List<GameObject>();

    private readonly string readDBURL = "https://oogb1rggj7.execute-api.us-east-1.amazonaws.com/Prod/readdb";
    public string writeDBURL = "https://oogb1rggj7.execute-api.us-east-1.amazonaws.com/Prod/writedb/"; 


    private void Start()
    {
        ClearList();
        //OnButtonSaveScore();

    }

    public void OnButtonSaveScore()
    {
        StartCoroutine(SaveUserScores());
    }

    public void OnButtonLoadScores()
    {
        StartCoroutine(GetUserScores());
    }

    IEnumerator GetUserScores()
    {
        ClearList();
        UnityWebRequest loadScoreRequest = UnityWebRequest.Get(readDBURL);

        yield return loadScoreRequest.SendWebRequest();
        if(loadScoreRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(loadScoreRequest.error);
            yield break;
        }
        else
        {
            Debug.Log("Read Scores Successfully");
        }

        JSONNode loadScoreInfo = JSON.Parse(loadScoreRequest.downloadHandler.text);
        JSONNode loadScoreItems = loadScoreInfo["Items"];

        string[] userIds = new string[loadScoreItems.Count];
        string[] scores = new string[loadScoreItems.Count];

        //string userIds = loadScoreItems["userId"];
        //string scores = loadScoreItems["score"];

        for (int i = 0; i < loadScoreItems.Count; i++)
        {
            userIds[i] = loadScoreItems[i]["userId"];
            scores[i] = loadScoreItems[i]["score"] + " %";

            userIdPrefabs[i].GetComponent<TMP_Text>().text = userIds[i];
            scorePrefabs[i].GetComponent<TMP_Text>().text = scores[i];


         //   scoreListUserIds.GetComponent<TMP_Text>().text += userIds[i];
         // scoreListScores.GetComponent<TMP_Text>().text += scores[i];

            Debug.Log(userIds[i]);
            Debug.Log(scores[i]);

        }
   
    }


    IEnumerator SaveUserScores()
    {
        //const string quote = "\"";
        string postData = gameManager.jsonData ;
        //WWWForm wWWForm = new WWWForm();

        //wWWForm.AddField("userId", gameManager.UserIdAndScore.userId);
        //wWWForm.AddField("score", gameManager.UserIdAndScore.score);
        var body = System.Text.Encoding.UTF8.GetBytes(postData);
        var saveScoresRequest = new UnityWebRequest(writeDBURL);
        saveScoresRequest.method = "POST";
        saveScoresRequest.uploadHandler = new UploadHandlerRaw(body);
        saveScoresRequest.downloadHandler = new DownloadHandlerBuffer();

        //UnityWebRequest saveScoresRequest = UnityWebRequest.Post(writeDBURL, "");
        
        Debug.Log(postData);
       // Debug.Log(wWWForm);

        //saveScoresRequest.SetRequestHeader("Content-Type", "application/json");
        yield return saveScoresRequest.SendWebRequest();
        if (saveScoresRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(saveScoresRequest.error);
            scoreSavedText.GetComponent<TMP_Text>().text = "There was an error saving the score :( ";
            yield break;
        }
        else
        {
            Debug.Log("Score saved Successfully");
            scoreSavedText.GetComponent<TMP_Text>().text = "Score saved Successfully ";
        }
    }

    private void ClearList()
    {
        for (int i = 0; i < userIdPrefabs.Length; i++)
        {
            userIdPrefabs[i].GetComponent<TMP_Text>().text = "";
            scorePrefabs[i].GetComponent<TMP_Text>().text = "";
        }
    }

}
