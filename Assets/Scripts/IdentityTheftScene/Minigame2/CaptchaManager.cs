using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CaptchaManager : MonoBehaviour
{
    public static CaptchaManager Instance
    {
        get;
        private set;
    }

    public GameObject[] captchaPrefab;
    public Transform captchaOrigin;

    private Canvas rend;
    private BoxCollider2D inputCollider;
    private Image spriteRenderer;
    private Animator entityAnimator;
    private AudioManager audioManager; 

    Toggle m_Toggle;
    private int selectedObjects;
    private int quiz = 0;
    private bool clear = false;

    private List<GameObject> captchaImage = new List<GameObject>();
    public List<int> captchaOrder = new List<int>();


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        SpawnCaptcha();
    }

    private void Update()
    {


    }

    private void SpawnCaptcha()
    {
        switch (quiz)
        {
            case 0:
                int z = 0;
                captchaOrder = new List<int>() { 0, 1, 2, 3, 4, 5 };
                // for the columns
                for (int i = 0; i <= 2; i++)
                {
                    // for the rows
                    for (int j = 0; j <= 1; j++)
                    {
                        Vector3 pos = new Vector3((i - 1) * 3.50f, (j - 0.8f) * 3.65f);
                        captchaImage.Add(Instantiate(captchaPrefab[quiz], pos, Quaternion.identity, captchaOrigin));
                        captchaImage[z].GetComponent<Captcha>().SetCaptcha();
                        z++;

                    }
                }
                break;

            case 1:
                z = 0;
                captchaOrder = new List<int>() { 0, 1, 2, 3, 4, 5 };
                captchaImage = new List<GameObject>();
                // for the columns
                for (int i = 0; i <= 2; i++)
                {
                    // for the rows
                    for (int j = 0; j <= 1; j++)
                    {
                        Vector3 pos = new Vector3((i - 1) * 3.50f, (j - 0.8f) * 3.65f);
                        captchaImage.Add(Instantiate(captchaPrefab[quiz], pos, Quaternion.identity, captchaOrigin));
                        captchaImage[z].GetComponent<Captcha>().SetCaptcha();
                        z++;

                    }
                }
                break;

            case 2:
                z = 0;
                captchaOrder = new List<int>() { 0, 1, 2, 3, 4, 5 };
                captchaImage = new List<GameObject>();
                // for the columns
                for (int i = 0; i <= 2; i++)
                {
                    // for the rows
                    for (int j = 0; j <= 1; j++)
                    {
                        Vector3 pos = new Vector3((i - 1) * 3.50f, (j - 0.8f) * 3.65f);
                        captchaImage.Add(Instantiate(captchaPrefab[quiz], pos, Quaternion.identity, captchaOrigin));
                        captchaImage[z].GetComponent<Captcha>().SetCaptcha();
                        z++;

                    }
                }
                break;
        }

    }



    public void CheckList()
    {
        int z = 0;

        for (int i = 0; i < captchaImage.Count; ++i)
        {
            if (!captchaImage[i].GetComponent<Captcha>().isSelected)
            {
                z += 1;
                continue;
            }
            else
            {
                if (!captchaImage[i].GetComponent<Captcha>().isBad)
                {
                    if (captchaImage[i].GetComponent<Captcha>().hasScored == false)
                    {
                        IdentityTheftManager_2.Instance.score += 100;
                        captchaImage[i].GetComponent<Captcha>().hasScored = true;
                        entityAnimator = captchaImage[i].GetComponent<Animator>();
                        entityAnimator.Play("Captcha_FlipR");
                    }
                }
                else
                {
                    if (captchaImage[i].GetComponent<Captcha>().hasScored == false)
                    {
                        IdentityTheftManager_2.Instance.score -= 100;
                        captchaImage[i].GetComponent<Captcha>().hasScored = true;
                    }
                }

                this.enabled = false;

            }
        }
        if(z != captchaImage.Count)
        {
            ++quiz;
            StartCoroutine(Clear());
        }

    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(30.0f);

        foreach (GameObject go in captchaImage)
        {
            Destroy(go);
        }

        captchaImage.Clear();

        clear = true;

        if (quiz <= 2)
        {        
           
            if (clear == true)
            {
                SpawnCaptcha();
                clear = false;
            }


        }
        else if (quiz > 2)
        {
            IdentityTheftManager_2.Instance.gameEnded = true;
        }
    }




}
