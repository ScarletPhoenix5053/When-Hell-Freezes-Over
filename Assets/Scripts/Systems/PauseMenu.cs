using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool pauseEnabled;
    public GameObject inventoryUI;

    public ItemTooltip tooltip, armorTooltip;

    public bool craftingEnabled;
    private PlayerInteract pB;
    public GameObject craftingUI;
    public GameObject notEnough;
    //public Canvas screenUI;

    void Start()
    {
        pB = FindObjectOfType<PlayerInteract>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!craftingEnabled)
            {
                pauseEnabled = !pauseEnabled;

                if (pauseEnabled)
                {
                    PausedGame();
                    inventoryUI.SetActive(true);
                    //screenUI.GetComponent<Canvas>().enabled = false;

                }
                else if (!pauseEnabled)
                {
                    UnpausedGame();
                    inventoryUI.SetActive(false);
                    //screenUI.GetComponent<Canvas>().enabled = true;
                    tooltip.HideTooltip();
                    armorTooltip.HideTooltip();
                }
            }
        }

        if(pB.atForge == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                craftingEnabled = !craftingEnabled;

                if(craftingEnabled)
                {
                    PausedGame();
                    pB.prompt.SetActive(false);
                    craftingUI.SetActive(true);
                    inventoryUI.SetActive(true);
                    notEnough.SetActive(true);
                }
                else if(!craftingEnabled)
                {
                    UnpausedGame();
                    craftingUI.SetActive(false);
                    notEnough.SetActive(false);
                    pB.prompt.SetActive(true);
                    inventoryUI.SetActive(false);
                    tooltip.HideTooltip();
                    armorTooltip.HideTooltip();
                }
   
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
        Debug.Log("Game is unpaused.");
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
