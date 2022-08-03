using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
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

        //shoppingScore = GameManager.INSTANCE.globalShoppingScore;
        //malwareScore = GameManager.INSTANCE.globalMalwareScore;
        //scamScore = GameManager.INSTANCE.globalScamScore;

    }

    private void Update()
    {
        
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
