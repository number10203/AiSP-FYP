using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class Minigame2Manager : MonoBehaviour
{
    // Public variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;

    public GameObject[] stars;

    public GameObject confettiParticle, stripesGameobject;

    public GameObject infographic;

    public GameObject infographicImage;
    public Sprite[] infographicLanguage;

    public Slider scoreSlider;

    public AudioClip BGM, starPop1, starPop2, starPop3, starEnd;

    private bool starPlay = false;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;

    [SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    [SerializeField] private GameObject minigame, resultsScreen, scoreUI;
    [SerializeField] private GameObject endCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private AudioClip loseAudio, winAudio;
    [SerializeField] private GameObject instructions1, instructions2, results;
    [SerializeField] private Sprite[] instructions1Language, instructions2Language;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] productLists;
    [SerializeField] private GameObject tick, cross;
    [SerializeField] private TMP_FontAsset CNFont, TMFont;

    private bool star1Anim = false, star2Anim = false, star3Anim = false;

    private int qnNumber = 0;
    private int correct = 0;
    private int localScore = 0, score;
    private int languageNumber;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameObject.GetComponent<ProductRandomizer>().RandomizeProducts(productLists);

        InitGameObjects();

        score = GameManager.INSTANCE.currentShoppingScore;
        audioManager.PlayMusic(BGM);

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            infographicImage.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
            instructions1.GetComponent<Image>().sprite = instructions1Language[languageNumber];
            instructions2.GetComponent<Image>().sprite = instructions2Language[languageNumber];
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            infographicImage.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
            instructions1.GetComponent<Image>().sprite = instructions1Language[languageNumber];
            instructions2.GetComponent<Image>().sprite = instructions2Language[languageNumber];
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            infographicImage.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
            instructions1.GetComponent<Image>().sprite = instructions1Language[languageNumber];
            instructions2.GetComponent<Image>().sprite = instructions2Language[languageNumber];
        }
        else
        {
            languageNumber = 0;
            infographicImage.GetComponent<Image>().sprite = infographicLanguage[languageNumber];

        }
    }

    private void FixedUpdate()
    {
        if (results.activeInHierarchy == true)
        {
            stripesGameobject.transform.localRotation *= Quaternion.Euler(0, 0, -1);
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                endScoreText.text = "<font=\"CHINA SDF1\">" + "总分: " + "</font>" + localScore;
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                endScoreText.text = "Jumlah Skor: " + localScore;
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                endScoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "ெமாத்த மதிப்பெண்: " + "</font>" + localScore;
            }
            else
            {
                endScoreText.text = "Total Score: " + localScore;
            }

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

    private void InitGameObjects()
    {
        star1Anim = false;
        star2Anim = false;
        star3Anim = false;

        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        minigame.SetActive(true);
        resultsScreen.SetActive(false);
        endCutscene.gameObject.SetActive(false);
        canvasGroup.blocksRaycasts = false;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(false);
        results.SetActive(false);
        scoreUI.SetActive(false);
        confettiParticle.SetActive(false);
        infographic.SetActive(false);

        foreach (GameObject gameObject in stars)
        {
            gameObject.SetActive(false);
        }
    }

    public void NextInstruction()
    {
        instructions1.SetActive(false);
        instructions2.SetActive(true);
    }

    public void PrevInstruction()
    {
        instructions1.SetActive(true);
        instructions2.SetActive(false);
    }

    public void StartGame()
    {
        instructions2.gameObject.SetActive(false);
        scoreUI.SetActive(true);
        canvasGroup.blocksRaycasts = true;
    }

    public void BuyProduct()
    {
        StartCoroutine(DoBuyProduct());
    }

    private IEnumerator DoBuyProduct()
    {
        canvasGroup.blocksRaycasts = false;

        GameObject buttonPressed = EventSystem.current.currentSelectedGameObject;
        GameObject ui = null;

        if (buttonPressed.transform.parent.CompareTag("ScamPurchase"))
        {
            ui = Instantiate(cross, buttonPressed.transform.parent.transform);
            audioManager.Play(wrongEffect);

            if (score <= 50)
            {
                score = 0;
            }
            else
            {
                score -= 100;
            }
        }
        else
        {
            ++correct;

            ui = Instantiate(tick, buttonPressed.transform.parent.transform);
            audioManager.Play(correctEffect);

            score += 150;
        }

        yield return new WaitForSeconds(1f);

        // Bring to next stage
        if (qnNumber == 2)
        {
            PlayCutscene();
        }
        else
        {
            for (int i = 0; i < productLists.Length; ++i)
            {
                LeanTween.moveLocalY(productLists[i], productLists[i].transform.localPosition.y + 800f, 1f).setEaseInOutBack();
                audioManager.Play(swooshEffect);
            }
        }

        ++qnNumber;

        yield return new WaitForSeconds(1f);

        canvasGroup.blocksRaycasts = true;
        Destroy(ui);
    }

    private void PlayCutscene()
    {
        audioManager.StopMusic();

        minigame.SetActive(false);
        resultsScreen.SetActive(true);
        endCutscene.SetActive(true);

        if (score >= 600)
        {
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene3_Chinese");
                subtitleManager.captions.font = CNFont;
                endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
                audioManager.Play(winAudio);
                StartCoroutine(StopCutscene(17f));
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene3_Malay");
                endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
                audioManager.Play(winAudio);
                StartCoroutine(StopCutscene(17f));
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene3_Tamil");
                subtitleManager.captions.font = TMFont;
                endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
                audioManager.Play(winAudio);
                StartCoroutine(StopCutscene(17f));
            }
            else
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene3_Eng");
                endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
                audioManager.Play(winAudio);
                StartCoroutine(StopCutscene(17f));
            }
        }
        else
        {
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene2_Chinese");
                subtitleManager.captions.font = CNFont;
                endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
                audioManager.Play(loseAudio);
                StartCoroutine(StopCutscene(6f));
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene2_Malay");
                endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
                audioManager.Play(loseAudio);
                StartCoroutine(StopCutscene(6f));
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene2_Tamil");
                subtitleManager.captions.font = TMFont;
                endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
                audioManager.Play(loseAudio);
                StartCoroutine(StopCutscene(6f));
            }
            else
            {
                subtitleManager.InitSubtitles("Jennie_Cutscene2_Eng");
                endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
                audioManager.Play(loseAudio);
                StartCoroutine(StopCutscene(6f));
            }
        }
    }

    private IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        endCutscene.SetActive(false);
        results.SetActive(true);

        if (score > GameManager.INSTANCE.globalShoppingScore)
        {
            GameManager.INSTANCE.globalShoppingScore = score;
        }
    }

    public void ShowInfoGraphic()
    {
        infographic.SetActive(true);
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
