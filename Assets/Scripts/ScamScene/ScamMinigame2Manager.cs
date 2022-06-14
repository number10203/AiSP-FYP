using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ScamMinigame2Manager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;
    public GameObject[] stars;
    public GameObject confettiParticle, stripesGameobject;
    public Slider scoreSlider;
   
    public AudioClip BGM, starPop1, starPop2, starPop3, starEnd;
    private bool starPlay = false;
    [SerializeField] private AudioClip correctEffect, wrongEffect, swooshEffect;

    [SerializeField] private GameObject minigame, resultsScreen, scoreUI;
    [SerializeField] private GameObject endCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private AudioClip loseAudio, winAudio;
    [SerializeField] private GameObject instructions1, instructions2, results;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] messageLists;
    [SerializeField] private GameObject tick, cross;

    private bool star1Anim = false, star2Anim = false, star3Anim = false;

    private int qnNumber = 0;
    private int correct = 0;
    private int localScore = 0, score;
    private AudioManager audioManager;


    // Start is called before the first frame update
    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
       

        InitGameObjects();

        score = GameManager.INSTANCE.currentShoppingScore;
        audioManager.PlayMusic(BGM);
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
    // Update is called once per frame
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

        scoreText.text = "Score: " + score;
    }
    private void InitGameObjects()
    {
        star1Anim = false;
        star2Anim = false;
        star3Anim = false;

        //startingFade.SetActive(true);
        //sceneTransition.SetActive(false);

        minigame.SetActive(true);
        resultsScreen.SetActive(false);
        //endCutscene.gameObject.SetActive(false);
        //canvasGroup.blocksRaycasts = false;
        instructions1.gameObject.SetActive(true);
        instructions2.gameObject.SetActive(false);
        results.SetActive(false);
        scoreUI.SetActive(false);
        confettiParticle.SetActive(false);
        //infographic.SetActive(false);

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
    public void ReplyMessage(int buttonID)
    {
        StartCoroutine(DoReplyMessage(buttonID));
    }

    private IEnumerator DoReplyMessage(int buttonID)
    {
        canvasGroup.blocksRaycasts = false;

        GameObject buttonPressed = EventSystem.current.currentSelectedGameObject;
        GameObject ui = null;
        switch(buttonID){
            case 1:
                GameObject.Find("ReplyButton1").SetActive(true);
                GameObject.Find("ReplyButton2").SetActive(true);
                GameObject.Find("TipText").SetActive(false);
                GameObject.Find("ConfirmButton").SetActive(false);
                break;
        }
        //if (buttonPressed.CompareTag("WrongReply"))
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

        //    score += 350;
        //}

        yield return new WaitForSeconds(1f);

        ////Bring to next stage
        //if (qnNumber == 2)
        //{
        //    PlayCutscene();
        //}
        //else
        //{
        //    messageLists[qnNumber+1].SetActive(true);
        //    LeanTween.moveLocalX(messageLists[qnNumber+1], messageLists[qnNumber+1].transform.localPosition.x + 1300f, 1f).setEaseInOutBack();
        //    LeanTween.moveLocalX(messageLists[qnNumber], messageLists[qnNumber].transform.localPosition.x + 1300f, 1f).setEaseInOutBack();
        //    audioManager.Play(swooshEffect);
        //    //for (int i = 0; i < messageLists.Length; ++i)
        //    //{
        //    //    LeanTween.moveLocalY(messageLists[i], messageLists[i].transform.localPosition.y + 800f, 1f).setEaseInOutBack();
        //    //    audioManager.Play(swooshEffect);
        //    //}
        //}

        //++qnNumber;

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
            subtitleManager.InitSubtitles("Jennie_Cutscene3_Eng");
            endCutscene.GetComponent<Animator>().Play("JennieWinCutscene");
            audioManager.Play(winAudio);
            StartCoroutine(StopCutscene(17f));
        }
        else
        {
            subtitleManager.InitSubtitles("Jennie_Cutscene2_Eng");
            endCutscene.GetComponent<Animator>().Play("JennieLoseCutscene");
            audioManager.Play(loseAudio);
            StartCoroutine(StopCutscene(6f));
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
}
