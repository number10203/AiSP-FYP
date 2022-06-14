using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class ScamManager_1 : MonoBehaviour
{
    //change to whack a mole

    // Public variables
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI endScoreText;
    public static ScamManager_1 Instance
    {
        get; private set;
    }

    //public GameObject confettiParticle, stripesGameobject;

    //public GameObject infographic;

    //public Slider scoreSlider;

    //public AudioClip BGM;

    // Private variables
    //[SerializeField] private GameObject startingFade, sceneTransition;

    //[SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    [SerializeField] private GameObject minigame, scoreUI;
    //[SerializeField] private GameObject endCutscene;
    //[SerializeField] private CutsceneSubtitleManager subtitleManager;
    //[SerializeField] private AudioClip loseAudio, winAudio;
    //[SerializeField] private GameObject instructions1, instructions2, results;
    //[SerializeField] private CanvasGroup canvasGroup;
    //[SerializeField] private GameObject[] productLists;
    //[SerializeField] private GameObject tick, cross;

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
   

        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {

        minigame.SetActive(true);
        //resultsScreen.SetActive(false);
        //results.SetActive(false);
        scoreUI.SetActive(true);

    }

    public void NextInstruction()
    {
        //instructions1.SetActive(false);
        //instructions2.SetActive(true);
    }

    public void PrevInstruction()
    {
        //instructions1.SetActive(true);
        //instructions2.SetActive(false);
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

    //private IEnumerator DoBuyProduct()
    //{
    //canvasGroup.blocksRaycasts = false;

    //GameObject buttonPressed = EventSystem.current.currentSelectedGameObject;
    //GameObject ui = null;

    //if (buttonPressed.transform.parent.CompareTag("ScamPurchase"))
    //{
    //    ui = Instantiate(cross, buttonPressed.transform.parent.transform);
    //    audioManager.Play(wrongEffect);

    //    if (score <= 50)
    //    {
    //        score = 0;
    //    }
    //    else
    //    {
    //        score -= 100;
    //    }
    //}
    //else
    //{
    //    ++correct;

    //    ui = Instantiate(tick, buttonPressed.transform.parent.transform);
    //    audioManager.Play(correctEffect);

    //    score += 150;
    //}

    //yield return new WaitForSeconds(1f);

    //// Bring to next stage
    //if (qnNumber == 2)
    //{
    //    PlayCutscene();
    //}
    //else
    //{
    //    for (int i = 0; i < productLists.Length; ++i)
    //    {
    //        LeanTween.moveLocalY(productLists[i], productLists[i].transform.localPosition.y + 800f, 1f).setEaseInOutBack();
    //        audioManager.Play(swooshEffect);
    //    }
    //}

    //++qnNumber;

    //yield return new WaitForSeconds(1f);

    //canvasGroup.blocksRaycasts = true;
    //Destroy(ui);
    //}





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
