using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Play("MainTheme");
    }

    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name.ToLower() == name.ToLower());

        if (s == null)
        {
            Debug.Log("Sound: " + name + "not found.");
            return;
        }

        s.source.Play();
    }

    //To put the sound in the code:
    // FindObjectOfType<AudioManager>().Play("sound");
}
