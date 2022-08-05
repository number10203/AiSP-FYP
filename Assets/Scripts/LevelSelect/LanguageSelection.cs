using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    public List<Sprite> supportedLanguageSprites = new List<Sprite>();
    private List<GameObject> languageOptions = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        foreach (Sprite language in supportedLanguageSprites)
        {
            GameObject prefab = new GameObject(language.name, typeof(Image), typeof(Button));
            GameObject obj = Instantiate(prefab, this.transform, false);
            languageOptions.Add(obj);
            Destroy(prefab);
            obj.name = language.name;
            obj.GetComponent<Image>().sprite = language;
            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;

            if (language.name != "EN")
                obj.SetActive(false);
            else
                obj.GetComponent<Button>().onClick.AddListener(OpenSelection);
        }
    }

    public void OpenSelection()
    {
        for (int i = 0; i < languageOptions.Count; i++)
        {
            GameObject obj = languageOptions[i];
            obj.SetActive(true);
            StartCoroutine(LerpRectTransformY(obj, -50f * i));
        }
    }

    private IEnumerator LerpRectTransformY(GameObject obj, float changeInPosition)
    {
        RectTransform transform = obj.GetComponent<RectTransform>();
        float targetYPos = transform.position.y + changeInPosition;

        if (changeInPosition > 0)
        {
            while (targetYPos > obj.GetComponent<RectTransform>().position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 10f * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (targetYPos < obj.GetComponent<RectTransform>().position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 10f * Time.deltaTime);
                yield return null;
            }
        }
        transform.position = new Vector3(transform.position.x, targetYPos);
    }
}
