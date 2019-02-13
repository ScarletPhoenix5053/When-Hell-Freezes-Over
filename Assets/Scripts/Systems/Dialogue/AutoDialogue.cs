using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDialogue : MonoBehaviour
{
    public string charName;
    private DialogueManager dMan;

    public string[] dialogueLines;


    void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
    }


    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!dMan.dialogActive)
            {
                dMan.dialogLines = dialogueLines;
                dMan.cName = charName;
                dMan.currentLine = 0;
                dMan.ShowDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
