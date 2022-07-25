using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Captcha : MonoBehaviour
{
    private Toggle toggle;
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
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void Update()
    {

    }

    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = Color.gray;
            cb.highlightedColor = Color.gray;
        }
        else
        {
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
        }
        toggle.colors = cb;
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

 

}
