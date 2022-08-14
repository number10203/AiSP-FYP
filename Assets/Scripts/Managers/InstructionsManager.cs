using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour
{
    public int languageNumber;

    internal bool instructionsFinished = false;
    internal bool isPlaying = false;
    
    [Header("Page Objects")]
    [SerializeField] private GameObject[] pages;
    [SerializeField] private Sprite[] instructionLanguagePg1, instructionLanguagePg2, instructionLanguagePg3;

    // Private variables
    private int pageNo = 0;


    private void Awake()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.CHINESE)
        {
            languageNumber = 1;
            pages[0].GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            pages[1].GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            pages[2].GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];
        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.MALAY)
        {
            languageNumber = 2;
            pages[0].GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            pages[1].GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            pages[2].GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];

        }
        else if (GameManager.INSTANCE.chosenLanguage == GameManager.LANGUAGE.TAMIL)
        {
            languageNumber = 3;
            pages[0].GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            pages[1].GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            pages[2].GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];

        }
        else
        {
            languageNumber = 0;
            pages[0].GetComponent<Image>().sprite = instructionLanguagePg1[languageNumber];
            pages[1].GetComponent<Image>().sprite = instructionLanguagePg2[languageNumber];
            pages[2].GetComponent<Image>().sprite = instructionLanguagePg3[languageNumber];

        }
    }

    public void StartInstructions()
    {
        isPlaying = true;
        pages[0].SetActive(true);
    }

    public void NextPage()
    {
        pages[pageNo].SetActive(false);

        ++pageNo;

        if (pageNo >= pages.Length)
        {
            instructionsFinished = true;
        }
        else
        {
            pages[pageNo].SetActive(true);
        }
    }

    public void PrevPage()
    {
        pages[pageNo].SetActive(false);

        --pageNo;

        pages[pageNo].SetActive(true);
    }
}
