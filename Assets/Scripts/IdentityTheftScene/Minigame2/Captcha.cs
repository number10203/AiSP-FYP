using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Captcha : MonoBehaviour
{
    public AudioClip correct, wrong;

    private Camera mainCam;
    private float CameraZDistance;

    public bool isSelected = false;
    public bool isBad = false;
    public bool hasScored = false;
    public int Length = 6;

    private IdentityTheftManager_2 manager;

    

    public static Captcha Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

    }


    // Set gameobject child to virus and change tag
    public void SetCaptcha()
    {

        int Rand = Random.Range(0, 6);
        if (CaptchaManager.Instance.captchaOrder.Contains(Rand))
        {
            transform.GetChild(Rand).gameObject.SetActive(true);
            if (Rand <= 2)
            {
                isBad = true;
            }

            CaptchaManager.Instance.captchaOrder.Remove(Rand);
        }
        else
        {
            SetCaptcha();
        }

        Debug.Log(Rand);
    }

    // Set gameobject child to virus and change tag
    public void SetSelect()
    {
        isSelected = !isSelected;
    }

    // When mouse is up, check if within distance of trashbin object
    // If within range, check if correct object
    void OnMouseUp()
    {



        //if (Vector3.Distance(transform.position, manager.trashbinPos) <= 2)
        //{
        //    GameObject temp = Instantiate(particleEffect, manager.trashbinPos, Quaternion.identity);
        //    Destroy(temp, 0.5f);

            //    if (gameObject.CompareTag("RealVirus"))
            //    {
            //        manager.score += 200;
            //        manager.correntAns++;

            //        manager.audioManager.Play(correct);

            //        spawner.virusCount--;
            //        gameObject.SetActive(false);
            //    }
            //    else if (gameObject.CompareTag("FakeVirus"))
            //    {
            //        if (manager.score > 50)
            //        {
            //            manager.score -= 100;
            //        }
            //        manager.wrongAns++;

            //        manager.audioManager.Play(wrong);

            //        spawner.virusCount--;
            //        gameObject.SetActive(false);
            //    }

            //    if ((manager.correntAns + manager.wrongAns) >= 3)
            //    {
            //        if (manager.correntAns > manager.wrongAns)
            //        {
            //            manager.isWin = true;
            //        }
            //        else
            //        {
            //            manager.isLose = true;
            //        }

            //        //manager.endPanel.SetActive(true);
            //    }
            //    else if (spawner.virusCount <= 2)
            //    {
            //        spawner.Clear();
            //        ++manager.qnNumber;
            //        spawner.Spawn();
            //    }
            //    else if (spawner.virusCount > 2)
            //    {
            //        spawner.Clear();
            //    }
            //}
    }

}
