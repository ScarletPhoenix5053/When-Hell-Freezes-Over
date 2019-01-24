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
    public Vector3 playerPosition;

    PauseMenu pauseMenu;

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

    public void Save()
    {
        //pauseMenu = GameObject.Find("GameManager").GetComponent<PauseMenu>();

        //if (pauseMenu.pauseEnabled == true)
        //{
            Debug.Log("Game is saved.");

            SaveGame.Instance.playerProgress = playerPosition;
            SaveGame.Save();
        //}
    }

    public void Load()
    {
        SaveGame.Load();

        Debug.Log("Game is loaded.");
        playerPosition = SaveGame.Instance.playerProgress;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
