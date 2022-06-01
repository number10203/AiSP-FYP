using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    internal bool isPreparing = false;
    internal bool hasPrepared = false;

    [SerializeField] internal GameObject cutsceneObj;
    [SerializeField] internal GameObject skipButton;

    public bool isPlaying = false;
    public bool hasPlayed = false;

    [SerializeField] private VideoPlayer videoPlayer;

    private void Update()
    {
        if (isPreparing && videoPlayer.isPrepared)
        {
            hasPrepared = true;
        }
    }

    public void PrepareCutscene(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Prepare();
        isPreparing = true;
    }

    public void StartCutscene()
    {
        if (isPlaying)
        {
            return;
        }

        cutsceneObj.SetActive(true);
        videoPlayer.loopPointReached += FinishCutscene;
        videoPlayer.Play();

        isPlaying = true;
    }

    public void StartCutscene(string url)
    {
        if (isPlaying)
        {
            return;
        }

        cutsceneObj.SetActive(true);
        videoPlayer.url = url;
        videoPlayer.loopPointReached += FinishCutscene;
        videoPlayer.Play();

        isPlaying = true;
    }

    private void FinishCutscene(VideoPlayer videoPlayer)
    {
        videoPlayer.Stop();
        isPlaying = false;
        hasPlayed = true;
    }

    public void SkipCutscene(int frame)
    {
        videoPlayer.frame = frame;
        skipButton.SetActive(false);
    }

    public void ResetCutscene()
    {
        videoPlayer.Stop();
        isPlaying = false;
        hasPlayed = false;
    }
}
