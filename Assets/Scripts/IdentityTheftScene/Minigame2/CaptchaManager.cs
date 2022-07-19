using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CaptchaManager : MonoBehaviour
{
    public GameObject captchaPrefab;
    public Transform captchaOrigin;

    private Canvas rend;
    private BoxCollider2D inputCollider;
    private Image spriteRenderer;
    private Animator entityAnimator;
    private AudioManager audioManager;

    Toggle m_Toggle;
    private int selectedObjects;

    private List<GameObject> captchaImage = new List<GameObject>();


    private void Start()
    {
        SpawnCaptcha();
    }

    private void Update()
    {
        //if (selected == true)
        //{
        //    if (addscore == true)
        //    {
        //        IdentityTheftManager_2.Instance.score += 100;
        //        addscore = false;
        //    }
        //}

    }

    private void SpawnCaptcha()
    {
        // for the columns
        for (int i = 0; i <= 2; i++)
        {
            // for the rows
            for (int j = 0; j <= 1; j++)
            {
                Vector3 pos = new Vector3((i - 1) * 5.25f , (j - 0.7f) * 3.5f);
                captchaImage.Add(Instantiate(captchaPrefab, pos, Quaternion.identity, captchaOrigin));
            }
        }

    }


    public void CheckList()
    {
        for (int i = 0; i < captchaImage.Count; ++i)
        {
            if (!captchaImage[i].GetComponent<Captcha>().isSelected)
                continue;
            else
            {
                if(!captchaImage[i].GetComponent<Captcha>().isBad)
                {
                    IdentityTheftManager_2.Instance.score += 100;
                    this.enabled = false;
                    Debug.Log(IdentityTheftManager_2.Instance.score);
                }
                
            }
        }
    }

    // Clear all folders
    public void Clear()
    {
        foreach (GameObject go in captchaImage)
        {
            Destroy(go);
        }

        captchaImage.Clear();
    }




}
