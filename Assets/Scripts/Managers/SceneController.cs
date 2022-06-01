using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region SingletonCreation
    // Single Instance creation
    private static SceneController _INSTANCE = null; 

    // Allow calling of singleton to other
    public static SceneController INSTANCE 
    {
        get
        {
            _INSTANCE = FindObjectOfType<SceneController>();
            if (_INSTANCE == null)
            {
                GameObject go = new GameObject();
                _INSTANCE = go.AddComponent<SceneController>();
                go.name = "SceneController";
                DontDestroyOnLoad(go);
            }
            return _INSTANCE;
        }
    }

    // Sets up singleton
    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Properly destroy singleton when quitting
    private void OnDestroy()
    {
        Destroy(gameObject);
        _INSTANCE = null;
        Debug.Log("Destroyed SceneController");
    }
    #endregion

    // Private variables
    private AsyncOperation asyncOperation;

    // Go to Scene 0 when ESC is pressed
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadScene(1);
        }
    }

    // Load Scenes with sceneIndex
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Load Scenes asynchronously with sceneIndex
    public void LoadSceneAsync(int sceneIndex)
    {
        if (asyncOperation == null)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            asyncOperation.allowSceneActivation = false;
        }
    }

    // Activate loaded Scenes
    public void ActivateLoadedScene()
    {
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = true;
            asyncOperation = null;
        }
    }

    // Restart Current Scene
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
