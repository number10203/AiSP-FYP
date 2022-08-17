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
    [SerializeField] private Sprite[] instructionsLanguage;
    [SerializeField] private GameObject results;
    [SerializeField] private TMP_FontAsset CNFont, TLFont;
    [SerializeField] private Sprite CNInfographic, MLInfographic, TLInfographic;
    [SerializeField] private Sprite[] instructionPages2 = new Sprite[3];

    [Header ("Audio References")]
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip startCutscene_1;
    private AudioManager audioManager;

    [Header ("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private float secondsUntilFinish;

    //[SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    private GameObject cutsceneAudio;

    internal int score = 0;
    private int counter = 0;
    private bool gameEnd = false;
    private int languageNumber;

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

        GameManager.INSTANCE.currentScamScore = 0;

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            infographic.GetComponent<Image>().sprite = CNInfographic;      

        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            infographic.GetComponent<Image>().sprite = MLInfographic;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            infographic.GetComponent<Image>().sprite = TLInfographic;
        }
        else
        {
            languageNumber = 0;
        }
    }

    private void FixedUpdate()
    {
        if (score < 0)
            score = 0;

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            scoreText.text = "<font=\"CHINA SDF1\">" + "分数: " + "</font>" + score;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            scoreText.text = "Skor: " + score;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            scoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "மதிப்பெண்: " + "</font>" + score;
        }
        else
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void Update()
    {
        if (minigame.activeInHierarchy)
        {
            secondsUntilFinish -= Time.deltaTime;
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                timerText.text = "<font=\"CHINA SDF1\">" + "剩余时间: " + "</font>" + (int)secondsUntilFinish + "s";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                timerText.text = "Masa yang tinggal: " + (int)secondsUntilFinish + "s";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                timerText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "ெமாத்த மதிப்பெண்: " + "</font>" + (int)secondsUntilFinish + "s";
            }
            else
            {
                timerText.text = "Time Left: " + (int)secondsUntilFinish + "s";
            }

            if (secondsUntilFinish <= 0.0f)
            {
                gameEnd = true;
            }
        }

        if (gameEnd)
        {
            results.SetActive(true);
            minigame.SetActive(false);
            GameManager.INSTANCE.currentScamScore = score;

            if (counter != score)
            {
                counter += 5;
                if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
                {
                    endScoreText.text = "<font=\"CHINA SDF1\">" + "总分: " + "</font>" + counter;
                }
                else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
                {
                    endScoreText.text = "Jumlah Skor: " + counter;
                }
                else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
                {
                    endScoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "ெமாத்த மதிப்பெண்: " + "</font>" + counter;
                }
                else
                {
                    endScoreText.text = "Total Score: " + counter;
                }
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
        cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            subtitleManager.InitSubtitles("AhHuat_Cutscene1_Chinese");
            subtitleManager.captions.font = CNFont;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            subtitleManager.InitSubtitles("AhHuat_Cutscene1_Malay");
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            subtitleManager.InitSubtitles("AhHuat_Cutscene1_Tamil");
            subtitleManager.captions.font = TLFont;
        }
        else
        {
            subtitleManager.InitSubtitles("AhHuat_Cutscene1_Eng");
        }
        StartCoroutine(TransitionToGame(30f));
    }

    private IEnumerator TransitionToGame(float time)
    {
        yield return new WaitForSeconds(time);

        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        startCutscene.SetActive(false);
        instructions.gameObject.SetActive(true);
        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            instructions.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Chinese");
            instructions.transform.GetChild(1).GetComponent<Image>().sprite = instructionPages2[languageNumber - 1];
            instructions.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(1339, 861);
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            instructions.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Malay");
            instructions.transform.GetChild(1).GetComponent<Image>().sprite = instructionPages2[languageNumber - 1];
            instructions.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(1339, 861);
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            instructions.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Tamil");
            instructions.transform.GetChild(1).GetComponent<Image>().sprite = instructionPages2[languageNumber - 1];
            instructions.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(1339, 861);
        }
        else
        {
            instructions.transform.GetChild(0).GetComponent<Animator>().SetTrigger("English");
        }
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

        if (score > GameManager.INSTANCE.globalScamScore)
        {
            GameManager.INSTANCE.globalScamScore = score;
        }
    }

    public void NextInstruction()
    {
        for (int i = 0; i < instructions.transform.childCount; i++)
        {
            if (instructions.transform.GetChild(i).gameObject.activeSelf && i < instructions.transform.childCount - 1)
            {
                instructions.transform.GetChild(i).gameObject.SetActive(false);
                instructions.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
        }
    }

    public void PreviousInstruction()
    {
        for (int i = 0; i < instructions.transform.childCount; i++)
        {
            if (instructions.transform.GetChild(i).gameObject.activeSelf && i > 0)
            {
                instructions.transform.GetChild(i).gameObject.SetActive(false);
                instructions.transform.GetChild(i - 1).gameObject.SetActive(true);
            }
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
