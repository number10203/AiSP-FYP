using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneSubtitleManager : MonoBehaviour
{
    // Cutscenes are 24 FPS

    [Header("Attributes")]
    [SerializeField] internal TextMeshProUGUI captions;

    private float timer = 0f;
    private bool captionsInitialized = false;

    private bool startSubtitle = false; // Used to stop update when finished

    // frame, caption
    private Dictionary<int[], string> captionsDictionary = new Dictionary<int[], string>();

    private void Update()
    {
        if (startSubtitle)
        {
            for (int i = captionsDictionary.Count - 1; i >= 0; --i)
            {
                int[] keys = captionsDictionary.Keys.ElementAt(i);
                if (timer >= keys[0] / 100 && timer <= keys[1] / 100)
                {
                    captions.text = captionsDictionary.Values.ElementAt(i);
                    break;
                }
                else
                {
                    captions.text = "";
                }
            }
        }

        if (captionsInitialized)
        {
            timer += Time.deltaTime;
        }
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    public void InitSubtitles(string filePath)
    {
        startSubtitle = true;
        TextAsset captionsFile = Resources.Load<TextAsset>(filePath);
        string captionsStr = captionsFile.text;

        string[] strArray = Regex.Split(captionsStr, "\n|\r|\r\n").Where(s => s != string.Empty).ToArray();

        for (int i = 0; i < strArray.Length; i += 2)
        {
            string[] timestamps = Regex.Split(strArray[i], " - ");

            // Start timestamp
            string[] startTimestamp = Regex.Split(timestamps[0], ":");
            int start = (int.Parse(startTimestamp[0]) * 100) + int.Parse(startTimestamp[1]);

            // End timestamp
            string[] endTimestamp = Regex.Split(timestamps[1], ":");
            int end = (int.Parse(endTimestamp[0]) * 100) + int.Parse(endTimestamp[1]);

            int[] timestampsInt = { start, end };

            captionsDictionary.Add(timestampsInt, strArray[i + 1]);
        }

        captionsInitialized = true;
    }

    public void FinishSubtitle()
    {
        startSubtitle = false;
        captionsInitialized = false;
    }
}
