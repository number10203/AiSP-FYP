using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class IdentityTheftManager_1 : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endScoreText;
    public GameObject infographic;
    public AudioClip BGM;
    public float timeRemaining;
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject startCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject results;

    [Header ("Audio References")]
    [SerializeField] private AudioClip startCutscene_1;
    private AudioManager audioManager;

    [Header ("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private GameObject minigameEnvironment;
    [SerializeField] private GameObject minigameProgressPanel;
    [SerializeField] private IdentityPlayerController player;
    [SerializeField] private float secondsUntilFinish;

    private GameObject cutsceneAudio;

    internal int score = 0;
    private int counter = 0;
    private bool gameEnd = false;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        InitGameObjects();

        GameManager.INSTANCE.currentIdentityScore = 0;
        Minigame1EventHandler.instance.onGameEnd += EndGame;
    }

    private void FixedUpdate()
    {
        if (score < 0)
            score = 0;

        scoreText.text = "Score: " + score;
        
    }

    private void Update()
    {
        if (minigame.activeInHierarchy)
        {
            //secondsUntilFinish -= Time.deltaTime;
            //timerText.text = "Time Left: " + (int) secondsUntilFinish + "s";
            UpdateProgress();

            if (secondsUntilFinish <= 0.0f)
            {
                //EndGame();
            }
        }
    }

    private void InitGameObjects()
    {
        // Init transitions
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        // Init cutscene
        startCutscene.SetActive(false);
        infographic.SetActive(false);
        instructions.SetActive(false);
        minigame.SetActive(true);
        results.SetActive(false);
        minigameEnvironment.SetActive(true);
        player.gameObject.SetActive(true);
        cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        subtitleManager.InitSubtitles("AhHuat_Cutscene1_Eng");
        //StartCoroutine(TransitionToGame(30f));
    }

    private void UpdateProgress()
    {
        TextMeshProUGUI[] texts = minigameProgressPanel.GetComponentsInChildren<TextMeshProUGUI>();
        //string password = "";
        foreach (GameObject character in player.characterList)
        {
            char[] SpecialChars = "!@#$%^&*()-_".ToCharArray();
            if (char.IsUpper(character.GetComponent<SpriteRenderer>().name[0]))
            {
                texts[1].fontStyle = FontStyles.Strikethrough;
            }
            if (character.GetComponent<SpriteRenderer>().name.IndexOfAny(SpecialChars) != -1)
            {
                texts[2].fontStyle = FontStyles.Strikethrough;
            }
        }
        if (player.characterList.Count >= 12)
        {
            texts[0].fontStyle = FontStyles.Strikethrough;
        }

        score = GameManager.INSTANCE.currentIdentityScore;
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");
        //Minigame1EventHandler.instance.CleanUpTrigger();
        results.SetActive(true);
        minigame.SetActive(false);
        score = GameManager.INSTANCE.currentIdentityScore;
        minigameEnvironment.SetActive(false);

        while (counter != score)
        {
            counter += 5;
            endScoreText.text = "Score: " + counter;
        }
    }

    private IEnumerator TransitionToGame(float time)
    {
        yield return new WaitForSeconds(time);

        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        startCutscene.SetActive(false);
        instructions.gameObject.SetActive(true);
        sceneTransition.SetActive(false);
        startingFade.SetActive(true);
        audioManager.PlayMusic(BGM);
    }

    public void SkipCutscene()
    {
        subtitleManager.SetTimer(22.00f);
        startCutscene.GetComponent<Cutscene>().SkipCutscene();
        startCutscene.GetComponent<Animator>().Play("AmirahStartingCutscene_Unskippable");

        Destroy(cutsceneAudio.gameObject);

        StopAllCoroutines();
        StartCoroutine(TransitionToGame(5f));
    }

    public void StartGame()
    {
        instructions.gameObject.SetActive(false);
        minigame.gameObject.SetActive(true);
        minigameEnvironment.SetActive(true);
        player.gameObject.SetActive(true);
    }

    private IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        //endCutscene.SetActive(false);
        //results.SetActive(true);

        if (score > GameManager.INSTANCE.globalScamScore)
        {
            GameManager.INSTANCE.globalScamScore = score;
        }
    }

    public void ShowInfoGraphic()
    {
        results.SetActive(false);
        infographic.SetActive(true);
    }
    public void NextMinigame()
    {
        sceneTransition.SetActive(true);
        SceneController.INSTANCE.LoadSceneAsync(9);
    }

    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
