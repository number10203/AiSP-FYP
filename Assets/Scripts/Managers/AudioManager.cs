using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Audio bools
    public bool hasDialogue = false;

    // Audio players components.
    public AudioSource MusicSource;
    public List<AudioSource> EffectsSource;
    public AudioSource DialogueSource;

    // Components
    [SerializeField] private DialogueManager dialogueManager;

    void Start()
    {
        if (GameObject.Find("MusicSource") != null)
        {
            MusicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Dialogue
        if (hasDialogue && !dialogueManager.isWriting)
        {
            DialogueSource.Stop();
        }

        // Effects
        for (int i = 0; i < EffectsSource.Count; ++i)
        {
            if (!EffectsSource[i].isPlaying)
            {
                Destroy(EffectsSource[i].gameObject);
                EffectsSource.Remove(EffectsSource[i]);
            }
        }
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        // Create new gameobject
        GameObject soundEffect = new GameObject("Sound Effect");
        AudioSource source = soundEffect.AddComponent<AudioSource>();
        EffectsSource.Add(source);

        source.clip = clip;
        source.Play();
    }

    // Play a single clip and get the gameobject
    public GameObject PlayAndGetObject(AudioClip clip)
    {
        // Create new gameobject
        GameObject soundEffect = new GameObject("Sound Effect");
        AudioSource source = soundEffect.AddComponent<AudioSource>();
        EffectsSource.Add(source);

        source.clip = clip;
        source.Play();

        return soundEffect;
    }

    public void PlayDialogueClip(AudioClip clip)
    {
        if (dialogueManager.isWriting && !DialogueSource.isPlaying)
        {
            DialogueSource.clip = clip;
            DialogueSource.Play();
        }
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
