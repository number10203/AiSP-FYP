using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class IdentityTheftManager_2 : MonoBehaviour
{
    public static IdentityTheftManager_2 Instance
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

    [Header("Audio References")]
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip startCutscene_1;
    public AudioManager audioManager;

    [Header("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private float secondsUntilFinish;

    //[SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    private GameObject cutsceneAudio;

    internal int score;
    private int counter = 0;
    private bool gameEnd = false;
    private bool toggleText = false;

    [HideInInspector] public bool isWin = false, isLose = false;

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

        score = GameManager.INSTANCE.currentIdentityScore;
    }

    private void FixedUpdate()
    {
        if (score < 0)
            score = 0;

        scoreText.text = "Score: " + score;

    }

    private void Update()
    {
        //if (minigame.activeInHierarchy)
        //{
        //    secondsUntilFinish -= Time.deltaTime;
        //    timerText.text = "Time Left: " + (int)secondsUntilFinish + "s";

        //    if (secondsUntilFinish <= 0.0f)
        //    {
        //        gameEnd = true;
        //    }
        //}

        if (gameEnd)
        {
            results.SetActive(true);
            minigame.SetActive(false);
            GameManager.INSTANCE.currentIdentityScore = score;

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
        //startingFade.SetActive(true);
        //sceneTransition.SetActive(false);

        // Init cutscene
        //startCutscene.SetActive(true);
        //infographic.SetActive(false);
        //instructions.SetActive(false);
        minigame.SetActive(true);
        results.SetActive(false);
        //cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        //subtitleManager.InitSubtitles("AhHuat_Cutscene1_Eng");
        //StartCoroutine(TransitionToGame(30f));
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
        startCutscene.GetComponent<Animator>().Play("AhHuatStartingCutscene_Unskippable");

        Destroy(cutsceneAudio.gameObject);

        StopAllCoroutines();
        StartCoroutine(TransitionToGame(5f));
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

        if (score > GameManager.INSTANCE.globalIdentityScore)
        {
            GameManager.INSTANCE.globalIdentityScore = score;
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

    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
