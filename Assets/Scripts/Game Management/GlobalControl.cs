using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    //This is where we save data, weapons, between scenes. Need to go through our scripts and find what we have.

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
