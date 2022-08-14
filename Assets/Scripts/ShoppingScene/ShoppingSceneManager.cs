using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Manages everything in shopping scene 1
// this thing is like REALLY messy its got like game logic, UI, everything
// we only realised that we should make like different managers for different things after we made this
// If u wanna reference the manager go to MalwareManager ples thank

public class ShoppingSceneManager : MonoBehaviour
{
    // Public variables
    public TextMeshProUGUI scoreText;

    public AudioClip music;

    public TextMeshProUGUI endScoreText;

    public GameObject infographic;
    public Sprite[] infographicLanguage;

    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public bool gameEnded = false;

    [HideInInspector] public int score = 0;

    // Private variables
    [SerializeField] private GameObject startingFade, sceneTransition;
    [SerializeField] private GameObject objectivesObj;
    [SerializeField] private GameObject endPanel;

    [SerializeField] private Sprite[] objectSprites;
    [SerializeField] private GameObject cutsceneObj;

    [SerializeField] private AudioClip startCutscene_1, startCutscene_2;
    [SerializeField] private Cutscene cutscene;
    [SerializeField] private CutsceneSubtitleManager cutsceneSubtitles;

    [SerializeField] private float ySpawn;

    private int counter; // Used to count the score up in the end screen.
    private GameObject cutsceneAudio;

    // Components
    private ObjectPool objectPool;
    private Gacha gacha;
    private AudioManager audioManager;
    private InstructionsManager instructionsManager;

    private int languageNumber;

    // Start is called before the first frame update
    void Start()
    {
        infographic.SetActive(false);
        GameManager.INSTANCE.currentShoppingScore = 0;
        counter = 0;
        objectPool = GameObject.Find("CurrentSceneManager").GetComponent<ObjectPool>();
        gacha = GameObject.Find("CurrentSceneManager").GetComponent<Gacha>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        instructionsManager = GetComponent<InstructionsManager>();

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            infographic.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            infographic.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            infographic.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
        }
        else
        {
            languageNumber = 0;
            infographic.GetComponent<Image>().sprite = infographicLanguage[languageNumber];
        }

        InitGameObjects();
    }

    private void Update()
    {
        if (endPanel.activeInHierarchy)
        {
            if (counter != score)
            {
                counter += 5;
                endScoreText.text = "Score: " + counter;
            }
        }
    }

    private void FixedUpdate()
    {
        scoreText.text = "Score: " + score;
    }

    private void InitGameObjects()
    {
        // Init transitions
        startingFade.SetActive(true);
        sceneTransition.SetActive(false);

        // Init cutscene
        cutsceneObj.SetActive(true);
        cutsceneAudio = audioManager.PlayAndGetObject(startCutscene_1);
        cutsceneSubtitles.InitSubtitles("Jennie_Cutscene1_Eng");
        StartCoroutine(TransitionToGame(30f));

        // Init GUI
        objectivesObj.SetActive(false);
        endPanel.SetActive(false);

 
    }

    private IEnumerator TransitionToGame(float time)
    {
        yield return new WaitForSeconds(time);

        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        cutsceneObj.SetActive(false);
        instructionsManager.StartInstructions();
        sceneTransition.SetActive(false);
        startingFade.SetActive(true);
        audioManager.PlayMusic(music);
    }

    public void SkipCutscene()
    {
        cutsceneSubtitles.SetTimer(27.05f);
        cutscene.SkipCutscene();
        cutsceneObj.GetComponent<Animator>().Play("JennieStartCutscene_Unskippable");

        Destroy(cutsceneAudio.gameObject);
        StopAllCoroutines();
        StartCoroutine(TransitionToGame(3f));
    }

    public void StartGame()
    {
        objectivesObj.SetActive(true);
        gameStarted = true;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (gameStarted && !gameEnded)
        {
            // Spawn products
            GameObject newObject = objectPool.GetPooledObject();
            if (newObject != null)
            {
                int productID = gacha.GetProductID();
                if (productID != -1)
                {
                    ProductStats stats = newObject.GetComponent<ProductStats>();
                    stats.SetStats(productID);
                    stats.RandomSpeed();

                    newObject.GetComponent<SpriteRenderer>().sprite = objectSprites[productID];

                    newObject.transform.position = new Vector3(Random.Range(-7f, 7f), ySpawn, 0);
                    newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));
                    newObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    public int GetNumberOfObjects()
    {
        return objectSprites.Length;
    }

    public void WinGame()
    {
        gameEnded = true;
        objectivesObj.SetActive(false);

        //audioManager.StopMusic();

        objectPool.SetAllToInactive();

        GameManager.INSTANCE.currentShoppingScore = score;

        endPanel.SetActive(true);
    }

    public void ShowInfoGraphic()
    {
        infographic.SetActive(true);
    }

    public void LoseGame()
    {
        gameEnded = true;
        objectivesObj.SetActive(false);
    }

    public void NextMinigame()
    {
        sceneTransition.SetActive(true);
        SceneController.INSTANCE.LoadSceneAsync(3);
    }

    public void RestartMinigame()
    {
        SceneController.INSTANCE.Retry();
    }
}
