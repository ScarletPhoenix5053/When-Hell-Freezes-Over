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
    public Canvas screenUI;
    public GameObject prompt;

    void Start()
    {
        pB = FindObjectOfType<PlayerInteract>();
    }


    void Update()
    {
        if (InputManager.Pause())
        {
            if (!craftingEnabled)
            {
                pauseEnabled = !pauseEnabled;

                if (pauseEnabled)
                {
                    PausedGame();
                    inventoryUI.SetActive(true);
                    screenUI.GetComponent<Canvas>().enabled = false;
                    prompt.SetActive(false);

                }
                else if (!pauseEnabled)
                {
                    UnpausedGame();
                    inventoryUI.SetActive(false);
                    screenUI.GetComponent<Canvas>().enabled = true;
                    tooltip.HideTooltip();
                    armorTooltip.HideTooltip();
                    //the object in itemslot has to shrink
                }
            }
        }

        if(pB.atForge == true)
        {
            if(InputManager.Interact())
            {
                craftingEnabled = !craftingEnabled;

                if(craftingEnabled)
                {
                    PausedGame();
                    pB.prompt.SetActive(false);
                    craftingUI.SetActive(true);
                    inventoryUI.SetActive(true);
                    screenUI.GetComponent<Canvas>().enabled = false;
                }
                else if(!craftingEnabled)
                {
                    UnpausedGame();
                    craftingUI.SetActive(false);
                    notEnough.SetActive(false);
                    pB.prompt.SetActive(true);
                    inventoryUI.SetActive(false);
                    screenUI.GetComponent<Canvas>().enabled = true;
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
