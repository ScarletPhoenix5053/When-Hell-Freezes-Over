using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool pauseEnabled;
    //public GameObject inventoryUI;
    //public Canvas screenUI;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseEnabled = !pauseEnabled;

            if (pauseEnabled)
            {
                PausedGame();
                //inventoryUI.SetActive(true);
                //screenUI.GetComponent<Canvas>().enabled = false;

            }
            else if (!pauseEnabled)
            {
                UnpausedGame();
               //inventoryUI.SetActive(false);
               //screenUI.GetComponent<Canvas>().enabled = true;
               //UIManager.GetComponent<UIManager>().HideTooltip();
            }
        }
    }

    public void PausedGame()
    {
        Debug.Log("Game is paused.");
        Time.timeScale = 0f;
    }

    public void UnpausedGame()
    {
        Time.timeScale = 1f;
    }
}
