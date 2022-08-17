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
    [SerializeField] private Sprite[] infographicLanguages;
    public AudioClip BGM;
    public float timeRemaining;
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject startCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private GameObject instructions;
    [SerializeField] private Sprite[] instructionLanguagePg1, instructionLanguagePg2, instructionLanguagePg3, instructionLanguagePg4;
    [SerializeField] private GameObject results;

    [Header ("Audio References")]
    [SerializeField] private AudioClip startCutscene_1;
    [SerializeField] private AudioClip collectSFX;
    private AudioManager audioManager;

    [Header ("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private GameObject minigameEnvironment;
    [SerializeField] private GameObject minigameCharacterPanel;
    [SerializeField] private GameObject minigameTypePanel;
    [SerializeField] private GameObject minigameStartPanel;
    [SerializeField] private IdentityPlayerController player;

    [SerializeField] private TMP_FontAsset CNFont;
    [SerializeField] private TMP_FontAsset TMFont;

    private GameObject cutsceneAudio;

    internal int score = 0;
    private float timer = 0;
    private int counter = 0;
    private bool gameEnd = false;
    public int languageNumber;
    private RawImage instructionImage;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        InitGameObjects();

        GameManager.INSTANCE.currentIdentityScore = 0;
        Minigame1EventHandler.instance.onGameEnd += EndGame;
        Minigame1EventHandler.instance.onEatCharacter += UpdateProgress;


        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            infographic.GetComponent<Image>().sprite = infographicLanguages[languageNumber];
            instructions.transform.Find("Page1").GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            instructions.transform.Find("Page2").GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            instructions.transform.Find("Page3").GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];
            instructions.transform.Find("Page4").GetComponent<Image>().sprite = instructionLanguagePg4[languageNumber];
            timerText.text = "消耗时间: " + (int)timer + "s";
            minigameCharacterPanel.transform.Find("score").GetComponent<TextMeshProUGUI>().text = "0/15\n字符";
            minigameTypePanel.transform.Find("UpperCheck").GetComponent<TextMeshProUGUI>().text = "大写";
            minigameTypePanel.transform.Find("LowerCheck").GetComponent<TextMeshProUGUI>().text = "小写";
            minigameTypePanel.transform.Find("NumeralCheck").GetComponent<TextMeshProUGUI>().text = "数字";
            minigameTypePanel.transform.Find("SymbolCheck").GetComponent<TextMeshProUGUI>().text = "符号";

        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            infographic.GetComponent<Image>().sprite = infographicLanguages[languageNumber];
            instructions.transform.Find("Page1").GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            instructions.transform.Find("Page2").GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            instructions.transform.Find("Page3").GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];
            instructions.transform.Find("Page4").GetComponent<Image>().sprite = instructionLanguagePg4[languageNumber];
            timerText.text = "Masa yang dihabiskan: " + (int)timer + "s";
            minigameCharacterPanel.transform.Find("score").GetComponent<TextMeshProUGUI>().text = "0/15\nHuruf";
            minigameTypePanel.transform.Find("UpperCheck").GetComponent<TextMeshProUGUI>().text = "Huruf besar";
            minigameTypePanel.transform.Find("LowerCheck").GetComponent<TextMeshProUGUI>().text = "Huruf kecil";
            minigameTypePanel.transform.Find("NumeralCheck").GetComponent<TextMeshProUGUI>().text = "Nombor";
            minigameTypePanel.transform.Find("SymbolCheck").GetComponent<TextMeshProUGUI>().text = "Simbol";

        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            infographic.GetComponent<Image>().sprite = infographicLanguages[languageNumber];
            instructions.transform.Find("Page1").GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            instructions.transform.Find("Page2").GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            instructions.transform.Find("Page3").GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];
            instructions.transform.Find("Page4").GetComponent<Image>().sprite = instructionLanguagePg4[languageNumber];
            timerText.text = "Time Spent: " + (int)timer + "s";

        }
        else
        {
            languageNumber = 0;
            infographic.GetComponent<Image>().sprite = infographicLanguages[languageNumber];
            instructions.transform.Find("Page1").GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            instructions.transform.Find("Page2").GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            instructions.transform.Find("Page3").GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];
            instructions.transform.Find("Page4").GetComponent<Image>().sprite = instructionLanguagePg4[languageNumber];
            timerText.text = "Time Spent: " + (int)timer + "s";
            minigameCharacterPanel.transform.Find("score").GetComponent<TextMeshProUGUI>().text = "0/15\nCharacters";
            minigameTypePanel.transform.Find("UpperCheck").GetComponent<TextMeshProUGUI>().text = "Uppercase";
            minigameTypePanel.transform.Find("LowerCheck").GetComponent<TextMeshProUGUI>().text = "Lowercase";
            minigameTypePanel.transform.Find("NumeralCheck").GetComponent<TextMeshProUGUI>().text = "Number";
            minigameTypePanel.transform.Find("SymbolCheck").GetComponent<TextMeshProUGUI>().text = "Symbol";

        }
    }

    private void FixedUpdate()
    {
        score = GameManager.INSTANCE.currentIdentityScore;
        if (score < 0)
            score = 0;

        //scoreText.text = "Score: " + score;
        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            scoreText.text = "分数: " + score;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            scoreText.text = "Skor: " + score;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            //scoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "மதிப்பெண்: " + "</font>" + score;
        }
        else
        {
            scoreText.text = "Score: " + score;
        }

    }

    private void Update()
    {
        if (minigame.activeInHierarchy && !gameEnd && player.GetComponent<IdentityPlayerController>().StartGame)
        {
            timer += Time.deltaTime;

            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                timerText.text = "消耗时间: " + (int)timer + "s";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                timerText.text = "Masa yang dihabiskan: " + (int)timer + "s";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                //timerText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "செலவிட்ட நேரம்: " + "</font>" + (int)timer + "s";
            }
            else
            {
                timerText.text = "Time Spent: " + (int)timer + "s";
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
        minigameEnvironment.SetActive(false);
        player.gameObject.SetActive(false);
        cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            subtitleManager.InitSubtitles("Amirah_Cutscene1_CN");
            subtitleManager.captions.font = CNFont;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            subtitleManager.InitSubtitles("Amirah_Cutscene1_BM");
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            subtitleManager.InitSubtitles("Amirah_Cutscene1_TM");
            subtitleManager.captions.font = TMFont;
        }
        else
        {
            subtitleManager.InitSubtitles("Amirah_Cutscene1_Eng");
        }
        StartCoroutine(TransitionToGame(28f));
    }

    private void UpdateProgress()
    {
        audioManager.Play(collectSFX);

        TextMeshProUGUI characterText = minigameCharacterPanel.GetComponentInChildren<TextMeshProUGUI>(false);
        Animator[] typeCollectionAnimators = minigameTypePanel.GetComponentsInChildren<Animator>();

        characterText.text = player.characterList.Count + "/15\nCharacters";
        if (player.characterList.Count >= 15)
        {
            EndGame();
            return;
        }

        foreach (GameObject characters in player.characterList)
        {
            CollectibleHandler characterHandler = characters.GetComponent<CollectibleHandler>();
            switch (characterHandler.type)
            {
                case CollectibleHandler.CharacterType.UPPERCASE:
                    if (!typeCollectionAnimators[0].GetCurrentAnimatorStateInfo(0).IsName("TypeCollected"))
                    {
                        GameManager.INSTANCE.currentIdentityScore += 50;
                        typeCollectionAnimators[0].SetTrigger("Collected");
                    }
                    break;
                case CollectibleHandler.CharacterType.LOWERCASE:
                    if (!typeCollectionAnimators[1].GetCurrentAnimatorStateInfo(0).IsName("TypeCollected"))
                    {
                        GameManager.INSTANCE.currentIdentityScore += 50;
                        typeCollectionAnimators[1].SetTrigger("Collected");
                    }
                    break;
                case CollectibleHandler.CharacterType.NUMERAL:
                    if (!typeCollectionAnimators[2].GetCurrentAnimatorStateInfo(0).IsName("TypeCollected"))
                    {
                        GameManager.INSTANCE.currentIdentityScore += 50;
                        typeCollectionAnimators[2].SetTrigger("Collected");
                    }
                    break;
                case CollectibleHandler.CharacterType.SYMBOL:
                    if (!typeCollectionAnimators[3].GetCurrentAnimatorStateInfo(0).IsName("TypeCollected"))
                    {
                        GameManager.INSTANCE.currentIdentityScore += 50;
                        typeCollectionAnimators[3].SetTrigger("Collected");
                    }
                    break;
            }
        }

        score = GameManager.INSTANCE.currentIdentityScore;
    }

    private void EndGame()
    {
        gameEnd = true;
        player.gameObject.SetActive(false);
        minigameStartPanel.SetActive(true);

        foreach (GameObject character in player.characterList)
        {
            Destroy(character);
        }
        if (player.characterList.Count < 15)
        {
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text ="游戏结束\n点击继续...";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Permainan tamat!\nKetik untuk teruskan...";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                //minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"NotoSansTamil-Bold SDF\">" + "கேம் ஓவர்!\nதொடர தட்டவும்..." + "</font>";
            }
            else
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Game Over!\nTap to continue...";
            }

        }
        else
        {
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text =  "你赢了！\n点击继续..." ;
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Awak menang!!\nKetik untuk teruskan...";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                //minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"NotoSansTamil-Bold SDF\">" + "நீ வெற்றி பெற்றாய்!!\nதொடர தட்டவும்..." + "</font>";
            }
            else
            {
                minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "You win!\nTap to continue...";
            }
        }
        StartCoroutine(ShowMinigameEndPanel());
    }

    private IEnumerator ShowMinigameEndPanel()
    {
        while (minigameStartPanel.activeSelf)
            yield return null;
        results.SetActive(true);
        minigameEnvironment.SetActive(false);
        minigame.SetActive(false);

        float multiplier = 1.8f;
        float timeOverMaxBonus = timer - 30f;

        if (timeOverMaxBonus % 5 != 0)
            timeOverMaxBonus -= timeOverMaxBonus % 5;
        multiplier -= (timeOverMaxBonus / 5 * 0.2f);
        if (multiplier <= 1f || player.characterList.Count < 15)
            multiplier = 1f;
        else if (multiplier > 1.8f)
            multiplier = 1.8f;
        player.characterList.Clear();

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            endScoreText.text =  "分数: "  + counter  + "\n时间倍数: " + multiplier + "x";
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            endScoreText.text = "Skor: " + counter + "\nPengganda masa: " + multiplier + "x";
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            //endScoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "மதிப்பெண்: " + "</font>" + counter + "<font=\"NotoSansTamil-Bold SDF\">" + "\nநேரம் பெருக்கி: " + "</font>" + multiplier + "x";
        }
        else
        {
            endScoreText.text = "Score: " + counter + "\nTime Multiplier: " + multiplier + "x";
        }

        GameManager.INSTANCE.currentIdentityScore = Mathf.RoundToInt(multiplier * GameManager.INSTANCE.currentIdentityScore / 10) * 10;
        score = GameManager.INSTANCE.currentIdentityScore;

        while (counter != score)
        {
            counter += 5;
            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
            {
                endScoreText.text =  "分数: " + counter + "\n时间倍数: " + multiplier + "x";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
            {
                endScoreText.text = "Skor: " + counter + "\nPengganda masa: " + multiplier + "x";
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
            {
                //endScoreText.text = "<font=\"NotoSansTamil-Bold SDF\">" + "மதிப்பெண்: " + "</font>" + counter + "<font=\"NotoSansTamil-Bold SDF\">" + "\nநேரம் பெருக்கி: " + "</font>" + multiplier + "x";
            }
            else
            {
                endScoreText.text = "Score: " + counter + "\nTime Multiplier: " + multiplier + "x";
            }
        }
        yield return null;
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
        subtitleManager.SetTimer(21.15f);
        startCutscene.GetComponent<Cutscene>().SkipCutscene();
        //startCutscene.GetComponent<Cutscene>().PlayUnskippable();
        startCutscene.GetComponent<Animator>().Play("AmirahStartingCutscene_Unskippable");

        Destroy(cutsceneAudio.gameObject);

        StopAllCoroutines();
        StartCoroutine(TransitionToGame(6f));
    }

    public void StartGame()
    {
        instructions.gameObject.SetActive(false);
        minigame.gameObject.SetActive(true);
        minigameEnvironment.SetActive(true);
        player.gameObject.SetActive(true);
    }

    public void TapToStart()
    {
        minigameStartPanel.SetActive(false);
        player.GetComponent<IdentityPlayerController>().StartGame = true;
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

    public void NextInstructions()
    {
        List<Transform> pages = new List<Transform>();
        foreach (Transform child in instructions.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.StartsWith("Page"))
                pages.Add(child);
        }

        foreach (Transform page in pages)
        {
            if (page.gameObject.activeSelf)
            {
                page.gameObject.SetActive(false);

                if (page.GetSiblingIndex() < pages.Count - 1)
                    pages[page.GetSiblingIndex() + 1].gameObject.SetActive(true);
                else
                    StartGame();
                return;
            }
        }
    }

    public void PreviousInstructions()
    {
        List<Transform> pages = new List<Transform>();
        foreach (Transform child in instructions.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.StartsWith("Page"))
                pages.Add(child);
        }

        foreach (Transform page in pages)
        {
            if (page.gameObject.activeSelf)
            {
                page.gameObject.SetActive(false);

                if (page.GetSiblingIndex() > 0)
                    pages[page.GetSiblingIndex() - 1].gameObject.SetActive(true);
                else
                    return;
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
        SceneController.INSTANCE.LoadSceneAsync(9);
    }

    public void RestartLevel()
    {
        SceneController.INSTANCE.Retry();
    }
}
