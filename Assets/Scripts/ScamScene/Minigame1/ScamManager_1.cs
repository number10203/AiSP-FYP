using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class ScamManager_1 : MonoBehaviour
{
    //TODO:
    //change to whack a mole

    // Public variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;
    public static ScamManager_1 Instance
    {
        get; private set;
    }

    //public GameObject confettiParticle, stripesGameobject;

    public GameObject infographic;

    public AudioClip BGM;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;

    [SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    [SerializeField] private GameObject minigame, scoreUI;
    [SerializeField] private GameObject startCutscene;
    [SerializeField] private GameObject endCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    //[SerializeField] private AudioClip loseAudio, winAudio;
    [SerializeField] private InstructionsManager instructionsManager;
    [SerializeField] private GameObject results;
    //[SerializeField] private CanvasGroup canvasGroup;

    internal int score = 0;
    private bool gameEnd = false;

    private AudioManager audioManager;

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

        //score = GameManager.INSTANCE.currentShoppingScore;
        //audioManager.PlayMusic(BGM);
    }

    private void FixedUpdate()
    {
        //if (results.activeInHierarchy == true)
        //{
        //    stripesGameobject.transform.localRotation *= Quaternion.Euler(0, 0, -1);
        //    endScoreText.text = "Total Score: " + localScore;

        //    if (localScore != score)
        //    {
        //        localScore += 10;

        //        scoreSlider.value = Mathf.Lerp(600, localScore, 1.0f);
        //    }
        //    else
        //    {
        //        if (score >= 600)
        //        {
        //            confettiParticle.SetActive(true);
        //        }

        //        //Play End Star sound here
        //        //audioManager.(starEnd);

        //        if (!starPlay)
        //        {
        //            audioManager.PlayAndGetObject(starEnd);
        //            starPlay = true;
        //        }
        //    }
        //}
    }

    private void Update()
    {
        if (score < 0)
            score = 0;

        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {
        // Init transitions
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        // Init cutscene
        startCutscene.SetActive(true);
        //cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        subtitleManager.InitSubtitles("Jennie_Cutscene1_Eng");
        StartCoroutine(TransitionToGame(30f));
    }

    private IEnumerator TransitionToGame(float time)
    {
        yield return new WaitForSeconds(time);

        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        startCutscene.SetActive(false);
        instructionsManager.StartInstructions();
        sceneTransition.SetActive(false);
        startingFade.SetActive(true);
        //audioManager.PlayMusic(music);
    }

    public void SkipCutscene()
    {
        //set cutscenesubtitle timer
        startCutscene.GetComponent<Cutscene>().SkipCutscene();
        startCutscene.GetComponent<Animator>().Play("JennieStartCutscene_Unskippable");
        //destroy OR stop audio related to cutscene
        StopAllCoroutines();
        StartCoroutine(TransitionToGame(3f));
        //start coroutine to transition to minigame
    }

    public void StartGame()
    {
        //instructions2.gameObject.SetActive(false);
        scoreUI.SetActive(true);
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

    private void PlayCutscene()
    {
        audioManager.StopMusic();

        minigame.SetActive(false);
        //resultsScreen.SetActive(true);
        //endCutscene.SetActive(true);

        //if (score >= 600)
        //{
        //    subtitleManager.InitSubtitles("Jennie_Cutscene3_Eng");
        //    endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
        //    audioManager.Play(winAudio);
        //    StartCoroutine(StopCutscene(17f));
        //}
        //else
        //{
        //    subtitleManager.InitSubtitles("Jennie_Cutscene2_Eng");
        //    endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
        //    audioManager.Play(loseAudio);
        //    StartCoroutine(StopCutscene(6f));
        //}
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
        //infographic.SetActive(true);
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
