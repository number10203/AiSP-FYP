using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class ScamManager_1 : MonoBehaviour
{
    public static ScamManager_1 Instance
    {
        get; private set;
    }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endScoreText;
    public GameObject infographic;
    public AudioClip BGM;
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject startCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject results;

    [Header ("Audio References")]
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private AudioClip winAudio;
    private AudioManager audioManager;

    [Header ("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private float secondsUntilFinish;

    //[SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;


    internal int score = 0;
    private int counter = 0;
    private bool gameEnd = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        InitGameObjects();
        //audioManager.PlayMusic(BGM);
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
            secondsUntilFinish -= Time.deltaTime;
            timerText.text = "Time Left: " + (int) secondsUntilFinish + "s";

            if (secondsUntilFinish <= 0.0f)
            {
                gameEnd = true;
            }
        }

        if (gameEnd)
        {
            results.SetActive(true);
            minigame.SetActive(false);

            if (counter != score)
            {
                counter += 5;
                endScoreText.text = "Score: " + counter;
            }
        }
    }

    private void InitGameObjects()
    {
        // Init transitions
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        // Init cutscene
        startCutscene.SetActive(true);
        infographic.SetActive(false);
        instructions.SetActive(false);
        minigame.SetActive(false);
        results.SetActive(false);
        //cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        subtitleManager.InitSubtitles("AhHuat_Cutscene1_Eng");
        StartCoroutine(TransitionToGame(30f));
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
        //audioManager.PlayMusic(music);
    }

    public void SkipCutscene()
    {
        subtitleManager.SetTimer(27.05f);
        startCutscene.GetComponent<Cutscene>().SkipCutscene();
        startCutscene.GetComponent<Animator>().Play("JennieStartCutscene_Unskippable");
        //destroy OR stop audio related to cutscene
        StopAllCoroutines();
        StartCoroutine(TransitionToGame(3f));
    }

    public void StartGame()
    {
        instructions.gameObject.SetActive(false);
        minigame.gameObject.SetActive(true);
        //canvasGroup.blocksRaycasts = true;
    }

    public void DoPickupCall()
    {
        StartCoroutine(PickupCall());
    }

    private IEnumerator PickupCall()
    {
        score += 50;
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        //endCutscene.SetActive(false);
        //results.SetActive(true);

        if (score > GameManager.INSTANCE.globalShoppingScore)
        {
            GameManager.INSTANCE.globalShoppingScore = score;
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
        SceneController.INSTANCE.LoadSceneAsync(7);
    }

    public void BackToMainMenu()
    {
        SceneController.INSTANCE.LoadScene(1);
    }

    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
