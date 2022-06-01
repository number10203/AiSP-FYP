using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneTestManager : MonoBehaviour
{
    public VideoPlayer cutscene;
    public CutsceneSubtitleManager cutsceneManager;

    // Start is called before the first frame update
    void Start()
    {
        //cutsceneManager.StartCutscene(System.IO.Path.Combine(Application.streamingAssetsPath, "Cutscenes/Jennie_Start.mp4"));
        cutsceneManager.InitSubtitles("Jennie_Cutscene1_Eng");
    }
}
