using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    public List<Sprite> supportedLanguageSprites = new List<Sprite>();
    public LevelSelectManager levelManager;
    private bool isOpening = false;
    private bool isToggled = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Sprite language in supportedLanguageSprites)
        {
            GameObject prefab = new GameObject(language.name, typeof(Image), typeof(Button));
            GameObject obj = Instantiate(prefab, this.transform, false);
            Destroy(prefab);
            obj.name = language.name;
            obj.GetComponent<Image>().sprite = language;
            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;

            if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.ENGLISH && obj.name == "EN")
            {
                obj.GetComponent<Button>().onClick.AddListener(ToggleSelection);                
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE && obj.name == "CN")
            {
                obj.GetComponent<Button>().onClick.AddListener(ToggleSelection);
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY && obj.name == "BM")
            {
                obj.GetComponent<Button>().onClick.AddListener(ToggleSelection);
            }
            else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL && obj.name == "TM")
            {
                obj.GetComponent<Button>().onClick.AddListener(ToggleSelection);
            }
        }
        Image[] languageOptions = this.GetComponentsInChildren<Image>(true);
        languageOptions[(int)GameManager.INSTANCE.chosenLanguage].transform.SetAsLastSibling();
    }

    public void ToggleSelection()
    {
        if (isOpening)
            return;

        Image[] languageOptions = this.GetComponentsInChildren<Image>(true);
        isOpening = true;
        isToggled = !isToggled;
        if (isToggled)
            for (int i = 0; i < languageOptions.Length; i++)
            {
                if (i == languageOptions.Length - 1)
                {
                    continue;
                }

                GameObject obj = languageOptions[i].gameObject;
                obj.SetActive(true);
                StartCoroutine(LerpRectTransformY(obj, -this.GetComponent<RectTransform>().sizeDelta.y * (i + 1), 1.1f));
                obj.GetComponent<Button>().onClick.AddListener(delegate { SelectOption(obj); });
            }
        else
            for (int i = 0; i < languageOptions.Length; i++)
            {
                if (i == languageOptions.Length - 1)
                {
                    continue;
                }

                GameObject obj = languageOptions[i].gameObject;
                obj.SetActive(true);
                StartCoroutine(LerpRectTransformY(obj, this.GetComponent<RectTransform>().sizeDelta.y * (i + 1), 1.1f));
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
            }
    }

    public void SelectOption(GameObject selectedObject)
    {
        if (isOpening)
            return;

        ToggleSelection();
        selectedObject.transform.SetAsLastSibling();

        Image[] languageOptions = this.GetComponentsInChildren<Image>(true);

        if(selectedObject.name == "CN")
        {
            GameManager.INSTANCE.chosenLanguage = GameManager.LANGUAGE.CHINESE;
        }
        else if (selectedObject.name == "BM")
        {
            GameManager.INSTANCE.chosenLanguage = GameManager.LANGUAGE.MALAY;           
        }
        else if (selectedObject.name == "TM")
        {
            GameManager.INSTANCE.chosenLanguage = GameManager.LANGUAGE.TAMIL;            
        }
        else
        {
            GameManager.INSTANCE.chosenLanguage = GameManager.LANGUAGE.ENGLISH;            
        }
        Debug.Log(GameManager.INSTANCE.chosenLanguage);
        levelManager.LanguageChange();

        selectedObject.GetComponent<Button>().onClick.AddListener(ToggleSelection);
    }
    private IEnumerator LerpRectTransformY(GameObject obj, float changeInPosition, float timeToTake)
    {
        RectTransform transform = obj.GetComponent<RectTransform>();
        float targetYPos = transform.anchoredPosition.y + changeInPosition;
        float timePassed = 0;

        if (changeInPosition > 0)
        {
            while (timePassed < timeToTake)
            {
                timePassed += Time.deltaTime;
                float newYPos = Mathf.Lerp(transform.anchoredPosition.y, targetYPos, timePassed / timeToTake);
                transform.anchoredPosition = new Vector3(transform.anchoredPosition.x, newYPos);
                isOpening = true;
                Debug.Log("Increasing! " + timePassed);
                yield return null;
            }
        }
        else
        {
            while (timePassed < timeToTake)
            {
                timePassed += Time.deltaTime;
                float newYPos = Mathf.Lerp(transform.anchoredPosition.y, targetYPos, timePassed / timeToTake);
                transform.anchoredPosition = new Vector3(transform.anchoredPosition.x, newYPos);
                isOpening = true;
                Debug.Log("Decreasing!" + timePassed);
                yield return null;
            }
        }
        transform.anchoredPosition = new Vector3(transform.anchoredPosition.x, targetYPos);
        isOpening = false;
    }
}
