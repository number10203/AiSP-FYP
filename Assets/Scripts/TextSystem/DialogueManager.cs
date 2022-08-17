using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    internal bool isWriting = false;

    // If the current dialogue manager is ready or is active = used to make sure it is loaded before using
    [HideInInspector] public bool isReady = false;
    [HideInInspector] public bool isActive = false;

    public Texture2D[] textureList;

    // Reading from text file
    [Header("Text File")]
    public TextAsset dialogueFile;

    // Used for streamingassets icons/sprites
    [Header("Icon Assets")]
    [SerializeField] private string[] iconList;

    // Dialogue shown name, message and sprites used
    [Header("GUI")]
    [SerializeField] private TextMeshProUGUI actorName;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject continueSprite;
    [SerializeField] private RawImage icon;

    [Header("SFX")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip audioClip;

    // Components in file
    private readonly List<Actor> actorList = new List<Actor>();
    private readonly List<Message> messageList = new List<Message>();

    // Displaying dialogue
    private int activeMessage = 0;

    private void Start()
    {
        // Display error if dialogue text is missing
        if (dialogueFile == null)
        {
            Debug.LogWarning("Dialogue file missing.");
        }
        else
        {
            ReadComponentsFromFile();
            StartCoroutine(GetIcons());
        }
    }

    // Reading of dialogue text file
    private void ReadComponentsFromFile()
    {
        string str = dialogueFile.text;
        string[] strArray = Regex.Split(str, "\n|\r|\r\n").Where(s => s != string.Empty).ToArray();

        if (strArray[0].Contains("[Actors]"))
        {
            string allActors = strArray[0].Remove(0, 8);
            string[] indivActors = Regex.Split(allActors, ",");
            for (int i = 0; i < indivActors.Length; ++i)
            {
                Actor actor = new Actor() { name = indivActors[i] };
                actorList.Add(actor);
            }
        }
        else
        {
            Debug.LogError("No Actors Found");
        }

        for (int i = 1; i < strArray.Length; ++i)
        {
            string[] messageArr = Regex.Split(strArray[i], ":");
            Message message = new Message() { actorID = int.Parse(messageArr[0]), messages = messageArr[1] };
            messageList.Add(message);
        }
    }

    // Collecting "Icons" from streamingassets connected to actorID
    private IEnumerator GetIcons()
    {
        textureList = new Texture2D[actorList.Count];

        for (int i = 0; i < actorList.Count; ++i)
        {
            Texture2D tex = new Texture2D(2, 2);
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Icons", iconList[i]);
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath))
            {
                yield return unityWebRequest.SendWebRequest();

                switch (unityWebRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                        tex.LoadImage(unityWebRequest.downloadHandler.data);
                        icon.texture = tex;
                        break;

                    default:
                        Debug.Log("UnityWebRequest failed.");
                        break;
                }
            }
            textureList[i] = tex;
        }

        isReady = true;
    }

    // Function to start dialogue (Used as a button or function)
    public void StartDialogue()
    {
        LeanTween.scale(dialogueBox, new Vector3(1, 1, 1), 1.0f).setEaseOutExpo();
        activeMessage = 0;

        isActive = true;

        DisplayMessage();
    }

    // Displays next message if any or ends the dialogue box when no more messages are next
    public void NextMessage()
    {
        if (isActive)
        {
            if (isWriting)
            {
                StopAllCoroutines();
                messageText.text = messageList.ToArray()[activeMessage].messages;
                continueSprite.SetActive(true);

                isWriting = false;
            }
            else
            {
                activeMessage++;
                if (activeMessage < messageList.ToArray().Length)
                {
                    DisplayMessage();
                }
                else
                {
                    LeanTween.scale(dialogueBox, new Vector3(0, 0, 0), 1.0f).setEaseOutExpo();
                    isActive = false;
                }
            }
        }
    }

    // Displays message out into TextMeshPro text box
    private void DisplayMessage()
    {
        continueSprite.SetActive(false);

        Message messageToDisplay = messageList.ToArray()[activeMessage];
        StartCoroutine(WriteMessageOut(messageToDisplay.messages));

        Actor actorToDisplay = actorList.ToArray()[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;

        icon.texture = textureList[messageToDisplay.actorID];
    }

    // Printing of text out to simulate typing
    private IEnumerator WriteMessageOut(string messsageText)
    {
        isWriting = true;

        audioManager.PlayDialogueClip(audioClip);

        string sentence = messsageText;
        messageText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        continueSprite.SetActive(true);

        isWriting = false;
    }
}

// Class creation
// Message to use
[System.Serializable]
public class Message
{
    public int actorID;

    [TextArea(2, 4)]
    public string messages;

    //public string[] messages;
}

// Actor name
[System.Serializable]
public class Actor
{
    public string name;
    //public Sprite avatar;
}
