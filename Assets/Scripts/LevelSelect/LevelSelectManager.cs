using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    // Public variables
    public GameObject[] shoppingStars, malwareStars, scamStars;

    // Private variables
    [SerializeField] private float buttonAnimScale = 1.2f;
    [SerializeField] private float buttonAnimDuration = 1f;
    [SerializeField] private GameObject sceneTransition;

    private int shoppingScore, malwareScore, scamScore;

    private SceneController sceneController;

    private void Start()
    {
        Time.timeScale = 1;

        sceneController = SceneController.INSTANCE;

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
            foreach (GameObject gameObject in malwareStars)
            {
                gameObject.SetActive(true);
            }
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
