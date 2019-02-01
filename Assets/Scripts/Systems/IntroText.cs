using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    public float delay = 0.1f;

    [TextArea(3, 10)]
    public string fullStory;
    private string currentStory = "";
    bool cutscene = false;

    //public Text text;
    void Start()
    {

    }

    public void TEffect()
    {
        if (!cutscene)
            StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        cutscene = true;
        for (int i = 0; i < fullStory.Length; i++)
        {
            currentStory = fullStory.Substring(0, i);
            this.GetComponent<Text>().text = currentStory;
            yield return new WaitForSeconds(delay);
        }

        cutscene = false;
    }
}

