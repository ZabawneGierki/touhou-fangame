using System.Collections.Generic;
using UnityEngine;


public enum ClipName
{
    Pichun
}
[System.Serializable]

public class SoundClip
{
    public ClipName name;
    public AudioClip Clip;
}
public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;




    // Singleton instance.
    public static SoundManager Instance = null;

    [SerializeField]
    public List<SoundClip> soundClips;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }




    // Play a single clip through the sound effects source.
    private void Play(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }


    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlaySoundByName(ClipName name)
    {
        SoundClip soundClip = soundClips.Find(clip => clip.name == name);
        if (soundClip != null)
        {
            Play(soundClip.Clip);
        }
        else
        {
            Debug.LogWarning($"SoundClip with name '{name}' not found!");
        }
    }


}