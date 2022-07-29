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
    [SerializeField] private GameObject minigameCharacterPanel;
    [SerializeField] private GameObject minigameTypePanel;
    [SerializeField] private GameObject minigameStartPanel;
    [SerializeField] private IdentityPlayerController player;

    private GameObject cutsceneAudio;

    internal int score = 0;
    private float timer = 0;
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
        if (minigame.activeInHierarchy && !gameEnd && player.GetComponent<IdentityPlayerController>().StartGame)
        {
            timer += Time.deltaTime;
            timerText.text = "Time Spent: " + (int) timer + "s";
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
        subtitleManager.InitSubtitles("Amirah_Cutscene1_Eng");
        StartCoroutine(TransitionToGame(28f));
    }

    private void UpdateProgress()
    {
        TextMeshProUGUI characterText = minigameCharacterPanel.GetComponentInChildren<TextMeshProUGUI>(false);
        Animator[] typeCollectionAnimators = minigameTypePanel.GetComponentsInChildren<Animator>();

        characterText.text = player.characterList.Count + "/15\nCharacters Collected";
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
            minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Game Over!\nTap to continue...";
        else
            minigameStartPanel.GetComponentInChildren<TextMeshProUGUI>().text = "You win!\nTap to continue...";
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
        endScoreText.text = "Score: " + counter + "\nTime Multiplier: " + multiplier + "x";

        GameManager.INSTANCE.currentIdentityScore = Mathf.RoundToInt(multiplier * GameManager.INSTANCE.currentIdentityScore / 10) * 10;
        score = GameManager.INSTANCE.currentIdentityScore;

        while (counter != score)
        {
            counter += 5;
            endScoreText.text = "Score: " + counter + "\nTime Multiplier: " + multiplier + "x";
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
