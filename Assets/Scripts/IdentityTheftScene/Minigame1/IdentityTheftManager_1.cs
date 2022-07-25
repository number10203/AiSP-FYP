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
    public AudioClip BGM;
    public float timeRemaining;
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject startCutscene;
    [SerializeField] private CutsceneSubtitleManager subtitleManager;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject results;

    [Header ("Audio References")]
    [SerializeField] private AudioClip startCutscene_1;
    private AudioManager audioManager;

    [Header ("Minigame References")]
    [SerializeField] private GameObject minigame;
    [SerializeField] private GameObject minigameEnvironment;
    [SerializeField] private GameObject minigameProgressPanel;
    [SerializeField] private IdentityPlayerController player;
    [SerializeField] private float secondsUntilFinish;

    private GameObject cutsceneAudio;

    internal int score = 0;
    private int counter = 0;
    private bool gameEnd = false;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        InitGameObjects();

        GameManager.INSTANCE.currentIdentityScore = 0;
        Minigame1EventHandler.instance.onGameEnd += EndGame;
        Minigame1EventHandler.instance.onEatCharacter += UpdateProgress;
    }

    private void FixedUpdate()
    {
        score = GameManager.INSTANCE.currentIdentityScore;
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
                EndGame();
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
        subtitleManager.InitSubtitles("AhHuat_Cutscene1_Eng");
        StartCoroutine(TransitionToGame(30f));
    }

    private void UpdateProgress()
    {
        int totalCharacters = 0;
        int uppercaseCharacters = 0;
        float percentageOfUppercase = 0.0f;
        TextMeshProUGUI[] texts = minigameProgressPanel.GetComponentsInChildren<TextMeshProUGUI>(false);
        foreach (GameObject character in player.characterList)
        {
            switch (character.GetComponent<CollectibleHandler>().type)
            {
                case CollectibleHandler.CharacterType.LOWERCASE:
                    totalCharacters++;
                    break;
                case CollectibleHandler.CharacterType.UPPERCASE:
                    totalCharacters++;
                    uppercaseCharacters++;
                    break;
                case CollectibleHandler.CharacterType.SYMBOL:
                    texts[2].text = "Collect one special symbol\n1/1 Special Symbol Remaining";
                    texts[2].color = new Color(0, 1, 0);
                    break;
            }
        }
        if (player.characterList.Count >= 12 && texts[0].fontStyle != FontStyles.Strikethrough)
        {
            texts[0].text = "Collect these amount of characters\n";
            texts[0].text += 12 - player.characterList.Count + " Characters Remaining";
            texts[0].fontStyle = FontStyles.Strikethrough;
            GameManager.INSTANCE.currentIdentityScore += 100;

            percentageOfUppercase = (float)uppercaseCharacters / (float)totalCharacters;
            if (percentageOfUppercase >= 0.5f)
            {
                texts[1].text = "Ensure that 50% of characters are uppercase upon getting 12 characters\n";
                texts[1].text += Mathf.RoundToInt(percentageOfUppercase * 100) + "% / 50% Uppercase Characters";
                texts[1].fontStyle = FontStyles.Strikethrough;
                texts[1].color = new Color(0, 1, 0);
                GameManager.INSTANCE.currentIdentityScore += 100;
            }
            else
            {
                GameManager.INSTANCE.currentIdentityScore -= 100;
            }
            if (texts[2].text == "Collect one special symbol\n1/1 Special Symbol Remaining")
            {
                GameManager.INSTANCE.currentIdentityScore += 100;
                texts[2].fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                GameManager.INSTANCE.currentIdentityScore -= 100;
            }
        }
        else if (texts[0].fontStyle != FontStyles.Strikethrough)
        {
            texts[0].text = "Collect these amount of characters\n";
            texts[0].text += 12 - player.characterList.Count + " Characters Remaining";

            if (uppercaseCharacters != 0)
                percentageOfUppercase = (float)uppercaseCharacters / (float)totalCharacters;
            texts[1].text = "Ensure that 50% of characters are uppercase upon getting 12 characters\n";
            texts[1].text += Mathf.RoundToInt(percentageOfUppercase * 100) + "% / 50% Uppercase Characters";
            if (percentageOfUppercase >= 0.5f)
                texts[1].color = new Color(0, 1, 0);
            else
                texts[1].color = new Color(1, 0, 0);
        }

        score = GameManager.INSTANCE.currentIdentityScore;
    }

    private void EndGame()
    {
        results.SetActive(true);
        minigame.SetActive(false);
        score = GameManager.INSTANCE.currentIdentityScore;
        player.gameObject.SetActive(false);
        minigameEnvironment.SetActive(false);

        while (counter != score)
        {
            counter += 5;
            endScoreText.text = "Score: " + counter;
        }
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
        startCutscene.GetComponent<Animator>().Play("AmirahStartingCutscene_Unskippable");

        Destroy(cutsceneAudio.gameObject);

        StopAllCoroutines();
        StartCoroutine(TransitionToGame(5f));
    }

    public void StartGame()
    {
        instructions.gameObject.SetActive(false);
        minigame.gameObject.SetActive(true);
        minigameEnvironment.SetActive(true);
        player.gameObject.SetActive(true);
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
