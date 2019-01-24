using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    //This is where we save data, weapons, between scenes.

    public int currentGold;
    public int forgesReached;

    public AudioMixer audioMixer;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
