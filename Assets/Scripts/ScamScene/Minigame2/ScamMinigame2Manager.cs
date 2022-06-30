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
    [SerializeField] private GameObject startingFade;
    [SerializeField] private GameObject minigame, resultsScreen, scoreUI, infoScreen;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject winCutscene, loseCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private AudioClip loseAudio, winAudio;
    [SerializeField] private GameObject results;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] MessageLists;
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

        score = GameManager.INSTANCE.currentScamScore;
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

        startingFade.SetActive(true);

        instructions.SetActive(true);
        minigame.SetActive(true);
        resultsScreen.SetActive(false);
        winCutscene.SetActive(false);
        loseCutscene.SetActive(false);
        canvasGroup.blocksRaycasts = false;
        results.SetActive(false);
        scoreUI.SetActive(false);
        confettiParticle.SetActive(false);
        infoScreen.SetActive(false);

        foreach (GameObject gameObject in stars)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        instructions.SetActive(false);
        canvasGroup.gameObject.SetActive(true);
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
                score += 100;
                MessageLists[23].SetActive(true);
                LeanTween.moveLocalY(MessageLists[23], MessageLists[23].transform.localPosition.y + 260f, .5f);
                yield return new WaitForSeconds(1.5f);
                //MessageLists[6].SetActive(true);
                //LeanTween.moveLocalY(MessageLists[6], MessageLists[6].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 190f, 1f);
                MessageLists[2].SetActive(true);
                LeanTween.moveLocalY(MessageLists[2], MessageLists[2].transform.localPosition.y + 100f, .5f);
                break;
            case 2:
                score += 150;
                MessageLists[24].SetActive(true);
                LeanTween.moveLocalY(MessageLists[24], MessageLists[24].transform.localPosition.y + 260f, .5f);
                yield return new WaitForSeconds(1.5f);
                //MessageLists[6].SetActive(true);
                //LeanTween.moveLocalY(MessageLists[6], MessageLists[6].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 190f, 1f);
                MessageLists[2].SetActive(true);
                LeanTween.moveLocalY(MessageLists[2], MessageLists[2].transform.localPosition.y + 100f, .5f);
                break;
            case 3:
                score += 150;
                MessageLists[25].SetActive(true);
                LeanTween.moveLocalY(MessageLists[25], MessageLists[25].transform.localPosition.y + 260f, .5f);
                yield return new WaitForSeconds(1.5f);
                //MessageLists[6].SetActive(true);
                //LeanTween.moveLocalY(MessageLists[6], MessageLists[6].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 190f, 1f);
                MessageLists[2].SetActive(true);
                LeanTween.moveLocalY(MessageLists[2], MessageLists[2].transform.localPosition.y + 100f, .5f);
                break;

            case 11:
                if (score - 100 < 0)
                    score = 0;
                else
                    score -= 100;
                Debug.Log("Replyed" + buttonID);
                MessageLists[3].SetActive(true);
                LeanTween.moveLocalY(MessageLists[3], MessageLists[3].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[6].SetActive(true);
                LeanTween.moveLocalY(MessageLists[6], MessageLists[6].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[9].SetActive(true);
                LeanTween.moveLocalY(MessageLists[9], MessageLists[9].transform.localPosition.y + 100f, .5f);
                break;
            case 12:
                score += 150;
                Debug.Log("Replyed" + buttonID);
                MessageLists[4].SetActive(true);
                LeanTween.moveLocalY(MessageLists[4], MessageLists[4].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[7].SetActive(true);
                LeanTween.moveLocalY(MessageLists[7], MessageLists[7].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[9].SetActive(true);
                LeanTween.moveLocalY(MessageLists[9], MessageLists[9].transform.localPosition.y + 100f, .5f);
                break;
            case 13:
                score += 150;
                Debug.Log("Replyed" + buttonID);
                MessageLists[5].SetActive(true);
                LeanTween.moveLocalY(MessageLists[5], MessageLists[5].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[8].SetActive(true);
                LeanTween.moveLocalY(MessageLists[8], MessageLists[8].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[9].SetActive(true);
                LeanTween.moveLocalY(MessageLists[9], MessageLists[9].transform.localPosition.y + 100f, .5f);
                break;
            case 21:
                if (score - 100 < 0)
                   score = 0;
                
                else
                    score -= 100;
                Debug.Log("Replyed" + buttonID);
                MessageLists[10].SetActive(true);
                LeanTween.moveLocalY(MessageLists[10], MessageLists[10].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[13].SetActive(true);
                LeanTween.moveLocalY(MessageLists[13], MessageLists[13].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[15].SetActive(true);
                LeanTween.moveLocalY(MessageLists[15], MessageLists[15].transform.localPosition.y + 100f, .5f);
                break;
            case 22:
                if (score - 100 < 0)
                    score = 0;

                else
                    score -= 100;
                Debug.Log("Replyed" + buttonID);
                MessageLists[11].SetActive(true);
                LeanTween.moveLocalY(MessageLists[11], MessageLists[11].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[21].SetActive(true);
                MessageLists[21].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                StartCoroutine(FadeImage(MessageLists[21]));
                yield return new WaitForSeconds(1.5f);
                MessageLists[22].SetActive(true);
                LeanTween.moveLocalY(MessageLists[22], MessageLists[22].transform.localPosition.y + 500f, .5f);
                //MessageLists[13].SetActive(true);
                //LeanTween.moveLocalY(MessageLists[13], MessageLists[13].transform.localPosition.y + 200f, .5f);
                //yield return new WaitForSeconds(2f);
                //LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                //MessageLists[15].SetActive(true);
                //LeanTween.moveLocalY(MessageLists[15], MessageLists[15].transform.localPosition.y + 100f, .5f);
                break;
            case 23:
                score += 150;
                Debug.Log("Replyed" + buttonID);
                MessageLists[12].SetActive(true);
                LeanTween.moveLocalY(MessageLists[12], MessageLists[12].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[14].SetActive(true);
                LeanTween.moveLocalY(MessageLists[14], MessageLists[14].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[15].SetActive(true);
                LeanTween.moveLocalY(MessageLists[15], MessageLists[15].transform.localPosition.y + 100f, .5f);
                break;
            case 24:
                if (score - 200 < 0)
                    score = 0;

                else
                    score -= 200;
                yield return new WaitForSeconds(1.5f);
                PlayCutscene();
                break;
            case 25:
                score += 100;
                yield return new WaitForSeconds(1.5f);
                PlayCutscene();
                break;
            case 31:
                if (score - 100 < 0)
                    score = 0;

                else
                    score -= 100;
                Debug.Log("Replyed" + buttonID);
                MessageLists[16].SetActive(true);
                LeanTween.moveLocalY(MessageLists[16], MessageLists[16].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[18].SetActive(true);
                LeanTween.moveLocalY(MessageLists[18], MessageLists[18].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[20].SetActive(true);
                LeanTween.moveLocalY(MessageLists[20], MessageLists[20].transform.localPosition.y + 100f, .5f);
                
                break;
            case 32:
                score += 100;
                Debug.Log("Replyed" + buttonID);
                MessageLists[17].SetActive(true);
                LeanTween.moveLocalY(MessageLists[17], MessageLists[17].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                MessageLists[19].SetActive(true);
                LeanTween.moveLocalY(MessageLists[19], MessageLists[19].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(2f);
                LeanTween.moveLocalY(MessageLists[1], MessageLists[1].transform.localPosition.y + 100f, 1f);
                MessageLists[20].SetActive(true);
                LeanTween.moveLocalY(MessageLists[20], MessageLists[20].transform.localPosition.y + 100f, .5f);
                
                break;
            case 41:
                if (score - 100 < 0)
                    score = 0;

                else
                    score -= 100;
                MessageLists[26].SetActive(true);
                LeanTween.moveLocalY(MessageLists[26], MessageLists[26].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                PlayCutscene();
                break;
            case 42:
                score += 100;
                MessageLists[27].SetActive(true);
                LeanTween.moveLocalY(MessageLists[27], MessageLists[27].transform.localPosition.y + 200f, .5f);
                yield return new WaitForSeconds(1.5f);
                PlayCutscene();
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
    IEnumerator FadeImage(GameObject img)
    {

        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.GetComponent<Image>().color = new Color(1, 1, 1, i);
            
            yield return null;
        }
        
    }
    private void PlayCutscene()
    {
        audioManager.StopMusic();

        minigame.SetActive(false);
        resultsScreen.SetActive(true);

        if (score >= 600)
        {
            subtitleManager.captions = winCutscene.GetComponentInChildren<TextMeshProUGUI>();
            subtitleManager.InitSubtitles("AhHuat_CutsceneWin_Eng");
            winCutscene.SetActive(true);
            audioManager.Play(winAudio);
            StartCoroutine(StopCutscene(32f));
        }
        else
        {
            subtitleManager.captions = loseCutscene.GetComponentInChildren<TextMeshProUGUI>();
            subtitleManager.InitSubtitles("AhHuat_Cutscene_Lose");
            loseCutscene.SetActive(true);
            audioManager.Play(loseAudio);
            StartCoroutine(StopCutscene(6f));
        }
    }

    private IEnumerator StopCutscene(float time)
    {
        yield return new WaitForSeconds(time);

        winCutscene.SetActive(false);
        loseCutscene.SetActive(false);
        results.SetActive(true);

        if (score > GameManager.INSTANCE.globalScamScore)
        {
            GameManager.INSTANCE.globalScamScore = score;
        }
    }
    public void ShowInfoGraphic()
    {
        results.SetActive(false);
        infoScreen.SetActive(true);
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
