using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject logo;

    private void Start()
    {
        //logo = GameObject.Find("SplashLogo");
        if (logo != null)
        {
            logo.SetActive(false);
        }
    }

    public void ShowLogo()
    {
        logo.SetActive(true);
    }

    public void ChangeScene()
    {
        SceneController.INSTANCE.ActivateLoadedScene();
    }

    public void OnSelf()
    {
        gameObject.SetActive(false);
    }
}
