using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dText;
    public Text dName;
    [HideInInspector] public string cName;

    public bool dialogActive;

    [HideInInspector] public string[] dialogLines;
    public int currentLine;
    //private PlayerController thePlayer;


    // Use this for initialization
    void Start()
    {
        //thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dialogActive && InputManager.Interact())
        {
            currentLine++;
        }

        if (currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;
        }

        dText.text = dialogLines[currentLine];
        dName.text = cName;

    }

    public void ShowDialogue()
    {
        dialogActive = true;
        dBox.SetActive(true);
    }


}
