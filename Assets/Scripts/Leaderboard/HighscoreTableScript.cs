using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighscoreTableScript : MonoBehaviour {
    public GameObject scoreText;
    public GameObject transition;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private string uploadURL = "http://127.0.0.1/backend/AddScore.php";
    private string downloadURL = "http://127.0.0.1/backend/ReadScoreboard.php";
    private string data;
    //public GameObject field1, field2, field3;
    private char[] alphabetArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    int globalTotalScore;
    private void Awake() {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        StartCoroutine(GetRequest(downloadURL));


        globalTotalScore = GameManager.INSTANCE.globalShoppingScore + GameManager.INSTANCE.globalMalwareScore
            + GameManager.INSTANCE.globalScamScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = globalTotalScore.ToString();
    }
    [Serializable]
    class OneScore
    {
        public string PlayerName;
        public int Score;
    }
    [Serializable]
    class ScoreList
    {
        public List<OneScore> scores = new List<OneScore>();
    }

    void Deserialize(String RawJSON)
    {
        ScoreList sb = JsonUtility.FromJson<ScoreList>(RawJSON); //convert raw json to objects
                                                                 //OPTIONAL: to show its really broken down into separate objects
        //displayTxt.text += "List Data:\n";
        for (int a = 0; a < sb.scores.Count; a++)
        {
            OneScore oneScore = sb.scores[a];
            Debug.Log(oneScore.PlayerName + "," + oneScore.Score);
            //displayTxt.text += (oneScore.PlayerName + "," + oneScore.Score + "\n");
        }
    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 45f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;

        entryTransform.Find("posText").GetComponent<Text>().text = rank.ToString();

        int score = highscoreEntry.Score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.PlayerName;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
     

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { Score = score, PlayerName = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                scores = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.scores.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }


    [System.Serializable]
    private class Highscores {
        public List<HighscoreEntry> scores;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    private class HighscoreEntry
    {
        public string PlayerName;
        public int Score;
    }

    public void AlphabetIncrease(GameObject field)
    {
        char[] AlphabetArray = field.GetComponent<TextMeshProUGUI>().text.ToCharArray();
        char AlphabetNow = AlphabetArray[0];
        int found = new String(alphabetArray).IndexOf(AlphabetNow);
        if (found == 25)
            found = -1;
        field.GetComponent<TextMeshProUGUI>().text = alphabetArray[found+1].ToString();
    }

    public void AlphabetDecrease(GameObject field)
    {
        char[] AlphabetArray = field.GetComponent<TextMeshProUGUI>().text.ToCharArray();
        char AlphabetNow = AlphabetArray[0];
        int found = new String(alphabetArray).IndexOf(AlphabetNow);
        if (found == 0)
            found = 26;
        field.GetComponent<TextMeshProUGUI>().text = alphabetArray[found - 1].ToString();
    }

    public void UploadScore()
    {
        //parameters = "&player=" + nameString + "&score=" + globalTotalScore;
        //action = "set";
        //AddHighscoreEntry(globalTotalScore, nameString);
        StartCoroutine(UploadScoreOnline());
    }

    private IEnumerator UploadScoreOnline()
    {
        string nameString = GameObject.Find("FirstLetterText").GetComponent<TextMeshProUGUI>().text +
              GameObject.Find("SecondLetterText").GetComponent<TextMeshProUGUI>().text +
              GameObject.Find("ThirdLetterText").GetComponent<TextMeshProUGUI>().text;
        WWWForm form = new WWWForm();
        form.AddField("player", nameString);
        form.AddField("score", globalTotalScore);
        UnityWebRequest webreq = UnityWebRequest.Post(uploadURL, form);
        yield return webreq.SendWebRequest();

        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                SceneController.INSTANCE.LoadSceneAsync(10);
                StartCoroutine(ActivateLoadedScene());
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    private IEnumerator ActivateLoadedScene()
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        SceneController.INSTANCE.ActivateLoadedScene();
    }

    private IEnumerator GetRequest(string URL)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest webreq = UnityWebRequest.Get(URL);
        yield return webreq.SendWebRequest();

        string receivedData = "";
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                receivedData = webreq.downloadHandler.text;
                break;
            default:
                Debug.Log("Error");
                break;
        }

        for (int i = 0; i < receivedData.Length; i++)
        {
            if (receivedData[i] == '\n')
            {
                data = receivedData.Substring(0, i + 1);
                break;
            }
        }
        Debug.Log(data);
        Highscores highscores = JsonUtility.FromJson<Highscores>(data);

        // Sort entry list by Score
        for (int i = 0; i < highscores.scores.Count; i++)
        {
            for (int j = i + 1; j < highscores.scores.Count; j++)
            {
                if (highscores.scores[j].Score > highscores.scores[i].Score)
                {
                    // Swap
                    HighscoreEntry tmp = highscores.scores[i];
                    highscores.scores[i] = highscores.scores[j];
                    highscores.scores[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < highscores.scores.Count; i++)
        {
            CreateHighscoreEntryTransform(highscores.scores[i], entryContainer, highscoreEntryTransformList);
        }
    }

}
