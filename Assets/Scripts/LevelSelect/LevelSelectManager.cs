using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Public variables
    public GameObject[] shoppingStars, malwareStars, scamStars, identityStars;
    public TextMeshProUGUI StoryText, JennieText, EthanText, AhHuatText, AmirahText;
    public TMP_FontAsset ENFont, CNFont, BMFont, TMFont;

    // Private variables
    [SerializeField] private float buttonAnimScale = 1.2f;
    [SerializeField] private float buttonAnimDuration = 1f;
    [SerializeField] private GameObject sceneTransition;

    private int shoppingScore, malwareScore, scamScore, identityScore;
    private int languageNumber;

    private SceneController sceneController;

    private void Start()
    {
        Time.timeScale = 1;

        sceneController = SceneController.INSTANCE;

        identityScore = GameManager.INSTANCE.globalIdentityScore;
        shoppingScore = GameManager.INSTANCE.globalShoppingScore;
        malwareScore = GameManager.INSTANCE.globalMalwareScore;
        scamScore = GameManager.INSTANCE.globalScamScore;

        foreach (GameObject i in shoppingStars)
        {
            i.SetActive(false);
        }

        foreach (GameObject i in malwareStars)
        {
            i.SetActive(false);
        }

        foreach (GameObject i in scamStars)
        {
            i.SetActive(false);
        }

        foreach (GameObject i in identityStars)
        {
            i.SetActive(false);
        }
    }

    private void Update()
    {
        //shoppingScore = GameManager.INSTANCE.shoppingScore1;

        if (shoppingScore >= 600 && shoppingScore < 750)
        {
            shoppingStars[0].SetActive(true);
        }
        else if (shoppingScore >= 750 && shoppingScore < 900)
        {
            shoppingStars[0].SetActive(true);
            shoppingStars[1].SetActive(true);
        }
        else if (shoppingScore >= 900)
        {
            foreach (GameObject gameObject in shoppingStars)
            {
                gameObject.SetActive(true);
            }
        }

        if (malwareScore >= 600 && malwareScore < 750)
        {
            malwareStars[0].SetActive(true);
        }
        else if (malwareScore >= 750 && malwareScore < 900)
        {
            malwareStars[0].SetActive(true);
            malwareStars[1].SetActive(true);
        }
        else if (malwareScore >= 900)
        {
            foreach (GameObject gameObject in malwareStars)
            {
                gameObject.SetActive(true);
            }
        }

        if (scamScore >= 600 && scamScore < 750)
        {
            scamStars[0].SetActive(true);
        }
        else if (scamScore >= 750 && scamScore < 900)
        {
            scamStars[0].SetActive(true);
            scamStars[1].SetActive(true);
        }
        else if (scamScore >= 900)
        {
            foreach (GameObject gameObject in scamStars)
            {
                gameObject.SetActive(true);
            }
        }

        if (identityScore >= 600 && identityScore < 750)
        {
            identityStars[0].SetActive(true);
        }
        else if (identityScore >= 750 && identityScore < 900)
        {
            identityStars[0].SetActive(true);
            identityStars[1].SetActive(true);
        }
        else if (identityScore >= 900)
        {
            foreach (GameObject gameObject in identityStars)
            {
                gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator FixTamilText(string textToFix, TextMeshProUGUI textComponent)
    {
        textComponent.text = textToFix;
        textComponent.font = TMFont;
        textComponent.GetComponent<CharReplacerTamil>().enabled = true;
        yield return null;
        textComponent.text = textComponent.GetComponent<CharReplacerTamil>().Convertedvalue;
    }

    public void LanguageChange()
    {
        //Language change
        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            StoryText.text = "故事选择";
            StoryText.font = CNFont;
            JennieText.text = "珍妮";
            JennieText.font = CNFont;
            EthanText.text = "伊桑";
            EthanText.font = CNFont;
            AhHuatText.text = "阿发";
            AhHuatText.font = CNFont;
            AmirahText.text = "阿米拉";
            AmirahText.font = CNFont;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            StoryText.text = "Pemilihan Cerita";
            StoryText.font = ENFont;
            JennieText.text = "Jennie";
            JennieText.font = ENFont;
            EthanText.text = "Ethan";
            EthanText.font = ENFont;
            AhHuatText.text = "Ah Huat";
            AhHuatText.font = ENFont;
            AmirahText.text = "Amirah";
            AmirahText.font = ENFont;
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            StartCoroutine(FixTamilText("கதையை தேர்ந்தெடுக்கவும்", StoryText));
            StartCoroutine(FixTamilText("ஜென்னி", JennieText));
            StartCoroutine(FixTamilText("ஈதன்", EthanText));
            StartCoroutine(FixTamilText("ஆ ஹுவாட்", AhHuatText));
            StartCoroutine(FixTamilText("அமிரா", AmirahText));
        }
        else
        {
            languageNumber = 0;
            StoryText.text = "Story Select";
            StoryText.font = ENFont;
            JennieText.text = "Jennie";
            JennieText.font = ENFont;
            EthanText.text = "Ethan";
            EthanText.font = ENFont;
            AhHuatText.text = "Ah Huat";
            AhHuatText.font = ENFont;
            AmirahText.text = "Amirah";
            AmirahText.font = ENFont;
        }
    }

    public void OnButtonPress(int sceneIndex)
    {
        // Play animation and start to load next scene
        LeanTween.scale(EventSystem.current.currentSelectedGameObject, new Vector3(buttonAnimScale, buttonAnimScale), buttonAnimDuration).setEasePunch();
        sceneController.LoadSceneAsync(sceneIndex);

        // Fade to black and change scenes after animation is done
        StartCoroutine(ActivateLoadedScene());
    }

    private IEnumerator ActivateLoadedScene()
    {
        sceneTransition.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        sceneController.ActivateLoadedScene();
    }
}
