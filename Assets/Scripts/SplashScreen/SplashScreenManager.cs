using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashScreenManager : MonoBehaviour
{
    // Public Variables
    public GameObject swipeTransition;

    public GameObject horizontalNotice;
    public GameObject splashLogoObject;

    // Private Variables
    private SceneController sceneController;
    private GameManager gameManager;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = SceneController.INSTANCE;
        gameManager = GameManager.INSTANCE;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        StartCoroutine(StartSplash());

        sceneController.LoadSceneAsync(1);
    }

    private IEnumerator StartSplash()
    {
        horizontalNotice.SetActive(true);
        splashLogoObject.SetActive(false);

        yield return new WaitForSeconds(5f);

        horizontalNotice.SetActive(false);
        splashLogoObject.SetActive(true);
    }
}
