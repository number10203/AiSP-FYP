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
    [SerializeField] private GameObject scoreUI, resultsScreen;
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI timerText;
    public TextMeshProUGUI endScoreText;
    public GameObject infographic;    
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject endCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject results;

    [Header("Audio References")]
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip startCutscene_1;
    public AudioClip BGM, starPop1, starPop2, starPop3, starEnd;
    public AudioManager audioManager;

    [Header("Minigame References")]
    [SerializeField] private GameObject minigame;
    //[SerializeField] private float secondsUntilFinish;


    private GameObject cutsceneAudio;

    public Slider scoreSlider;
    public GameObject[] stars;
    public GameObject confettiParticle, stripesGameobject;
    private bool star1Anim = false, star2Anim = false, star3Anim = false;
    private bool starPlay = false;


    internal int localScore = 0, score;
    private int counter = 0;
    private bool toggleText = false;

    [HideInInspector] public bool isWin = false, isLose = false, gameEnded = false ;

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
        if (results.activeInHierarchy == true)
        {
            stripesGameobject.transform.localRotation *= Quaternion.Euler(0, 0, -1);
            endScoreText.text = "Total Score: " + localScore;

            if (localScore != score)
            {
                localScore += 10;

                scoreSlider.value = Mathf.Lerp(600, localScore, 1.0f);
            }
            else
            {
                if (score >= 600)
                {
                    confettiParticle.SetActive(true);
                }

                //Play End Star sound here
                //audioManager.(starEnd);

                if (!starPlay)
                {
                    audioManager.PlayAndGetObject(starEnd);
                    starPlay = true;
                }
            }
        }



    }

    private void Update()
    {
        if(gameEnded == true)
        {
            StartCoroutine(StopCutscene(0.0f));
        }

        if (localScore >= 900)
        {
            if (star3Anim != true)
            {
                stars[2].SetActive(true);
                stars[2].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star3Anim = true;

                audioManager.PlayAndGetObject(starPop3);
            }
        }
        else if (localScore >= 750)
        {
            if (star2Anim != true)
            {
                stars[1].SetActive(true);
                stars[1].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star2Anim = true;

                audioManager.PlayAndGetObject(starPop2);
            }
        }
        else if (localScore >= 600)
        {
            if (star1Anim != true)
            {
                stars[0].SetActive(true);
                stars[0].LeanScale(new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
                star1Anim = true;

                audioManager.PlayAndGetObject(starPop1);
            }
        }

        if (score < 0)
            score = 0;

        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {
        star1Anim = false;
        star2Anim = false;
        star3Anim = false;

        startingFade.SetActive(true);

        instructions.SetActive(true);
        minigame.SetActive(true);
        resultsScreen.SetActive(false);
        //winCutscene.SetActive(false);
        //loseCutscene.SetActive(false);        
        results.SetActive(false);
        //scoreUI.SetActive(false);
        confettiParticle.SetActive(false);
        infographic.SetActive(false);

        foreach (GameObject gameObject in stars)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TransitionToGame(float time)
    {
        yield return new WaitForSeconds(time);

        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        instructions.gameObject.SetActive(true);
        sceneTransition.SetActive(false);
        startingFade.SetActive(true);
        audioManager.PlayMusic(BGM);
    }


    public void StartGame()
    {
        instructions.gameObject.SetActive(false);
        minigame.gameObject.SetActive(true);
        //canvasGroup.blocksRaycasts = true;
    }


    private IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        endCutscene.SetActive(false);
        results.SetActive(true);

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

    public void BackToMainMenu()
    {
        SceneController.INSTANCE.LoadScene(1);
        GameManager.INSTANCE.currentIdentityScore = 0;
    }
    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
