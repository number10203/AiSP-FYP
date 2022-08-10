using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    public List<Sprite> supportedLanguageSprites = new List<Sprite>();
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

            if (language.name == "EN")
            {
                obj.GetComponent<Button>().onClick.AddListener(ToggleSelection);
            }
        }
        Image[] languageOptions = this.GetComponentsInChildren<Image>(true);
        languageOptions[0].transform.SetAsLastSibling();
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
                StartCoroutine(LerpRectTransformY(obj, -this.GetComponent<RectTransform>().sizeDelta.y * (i + 1), 1f));
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
                StartCoroutine(LerpRectTransformY(obj, this.GetComponent<RectTransform>().sizeDelta.y * (i + 1), 1f));
                obj.GetComponent<Button>().onClick.AddListener(delegate { SelectOption(obj); });
            }
    }

    public void SelectOption(GameObject selectedObject)
    {
        if (isOpening)
            return;

        ToggleSelection();
        selectedObject.transform.SetAsLastSibling();

        Image[] languageOptions = this.GetComponentsInChildren<Image>(true);
        foreach (Image image in languageOptions)
        {
            image.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        selectedObject.GetComponent<Button>().onClick.AddListener(ToggleSelection);
    }
    private IEnumerator LerpRectTransformY(GameObject obj, float changeInPosition, float timeToTake)
    {
        RectTransform transform = obj.GetComponent<RectTransform>();
        float targetYPos = transform.position.y + changeInPosition;
        float timePassed = 0;

        if (changeInPosition > 0)
        {
            while (targetYPos > obj.GetComponent<RectTransform>().position.y)
            {
                timePassed += Time.deltaTime;
                float newYPos = Mathf.Lerp(transform.position.y, targetYPos, timePassed / timeToTake);
                transform.position = new Vector3(transform.position.x, newYPos);
                yield return null;
            }
        }
        else
        {
            while (targetYPos < obj.GetComponent<RectTransform>().position.y)
            {
                timePassed += Time.deltaTime;
                float newYPos = Mathf.Lerp(transform.position.y, targetYPos, timePassed / timeToTake);
                transform.position = new Vector3(transform.position.x, newYPos);
                yield return null;
            }
        }
        transform.position = new Vector3(transform.position.x, targetYPos);
        isOpening = false;
    }
}
