using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    internal bool instructionsFinished = false;
    internal bool isPlaying = false;
    
    [Header("Page Objects")]
    [SerializeField] private GameObject[] pages;

    // Private variables
    private int pageNo = 0;

    private void Awake()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
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
