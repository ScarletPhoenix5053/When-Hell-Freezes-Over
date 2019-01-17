using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    public string charName;
    private DialogueManager dManager;

    [TextArea(2, 10)]
    public string[] dialogueLines;


    void Start()
    {
        dManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {

            if (Input.GetKeyDown(KeyCode.Space)) //If it's getkeydown the first line doesn't play, but it doesn't repeat. 
            {
                //Audio here?

                if (dManager.dialogActive == false)
                {
                    dManager.dialogLines = dialogueLines;
                    dManager.cName = charName;
                    dManager.currentLine = 0;
                    dManager.ShowDialogue();
                }

            }
        }
    }

}
