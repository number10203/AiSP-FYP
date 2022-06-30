using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip unskippable;
    [SerializeField] private GameObject skipButton;

    // Start is called before the first frame update
    void Start()
    {
        if (skipButton)
        {
            skipButton.SetActive(true);
        }
    }

    public void PlayUnskippable()
    {
        if (unskippable == null)
        {
            Debug.LogError("No unskippable dialogue provided");
            return;
        }
        //audioManager.PlayOneShot(unskippable);
        GetComponent<AudioSource>().PlayOneShot(unskippable);
       
    }

    public void SkipCutscene()
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Stop();
        skipButton.SetActive(false);
    }
}
