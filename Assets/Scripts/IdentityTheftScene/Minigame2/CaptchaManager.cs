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
    public AudioClip cardFlip;
    public AudioManager audioManager;

    private int selectedObjects;
    private int quiz = 0;
    private int languageNumber;
    private bool clear = false;
    private bool checkPost = false;

    private List<GameObject> captchaImage = new List<GameObject>();
    public List<int> captchaOrder = new List<int>();


    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        languageNumber = IdentityTheftManager_2.Instance.languageNumber * 2;

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
                        captchaImage.Add(Instantiate(captchaPrefab[quiz + languageNumber], pos, Quaternion.identity, captchaOrigin));
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
                        captchaImage.Add(Instantiate(captchaPrefab[quiz + languageNumber], pos, Quaternion.identity, captchaOrigin));
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
            }
            else
            {
                if (captchaImage[i].GetComponent<Captcha>().hasScored == false)
                {
                    if (!captchaImage[i].GetComponent<Captcha>().isBad)
                    {
                        IdentityTheftManager_2.Instance.score += 100;
                        captchaImage[i].GetComponent<Captcha>().hasScored = true;
                        entityAnimator = captchaImage[i].GetComponent<Animator>();
                        entityAnimator.Play("Captcha_FlipR");
                        audioManager.Play(cardFlip);
                    }
                    else
                    {
                        IdentityTheftManager_2.Instance.score -= 100;
                        captchaImage[i].GetComponent<Captcha>().hasScored = true;
                        entityAnimator = captchaImage[i].GetComponent<Animator>();
                        entityAnimator.Play("Captcha_FlipW");
                        audioManager.Play(cardFlip);
                    }
                }                
            }
            captchaImage[i].GetComponent<Toggle>().interactable = false;
        }
        if(z != captchaImage.Count && !checkPost)
        {
            ++quiz;
            checkPost = true;
            StartCoroutine(Clear());
        }
        else if (z == captchaImage.Count && !checkPost)
        {
            for (int i = 0; i < captchaImage.Count; ++i)
            {
                captchaImage[i].GetComponent<Toggle>().interactable = true;
            }
        }

    }

    IEnumerator Clear()
    {

        yield return new WaitForSeconds(3.0f);

        foreach (GameObject go in captchaImage)
        {
            Destroy(go);
        }

        captchaImage.Clear();
        checkPost = false;
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
            IdentityTheftManager_2.Instance.PlayCutscene();
        }
    }




}
