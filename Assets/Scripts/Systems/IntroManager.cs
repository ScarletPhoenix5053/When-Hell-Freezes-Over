using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Text[] text;
    public Image[] image;
    public GameObject[] animatedPieces;
    public string levelToLoad;

    public IntroText[] TE;

    void Start()
    {
        StartCoroutine(ShowCutscenes());
    }

    void Update()
    {
        SkipScene();
    }

    IEnumerator ShowCutscenes()
    {
        //Cutscene 1
        image[0].enabled = true;
        animatedPieces[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        text[0].enabled = true;
        TE[0].TEffect();
        yield return new WaitForSeconds(7f);

        //Fade Transition 1 to 2
        image[1].enabled = true; //Enable the next image
        animatedPieces[0].SetActive(false); //Set the old active pieces false
        text[0].enabled = false; //Set the old text false
        image[0].CrossFadeAlpha(0, .5f, false); //Set the old image to fade out
        yield return new WaitForSeconds(1f);

        //Cutscene 2
        text[1].enabled = true; //Enable the next text + effect
        TE[1].TEffect();
        yield return new WaitForSeconds(1f);
        animatedPieces[1].SetActive(true); //Set the animated pieces true
        yield return new WaitForSeconds(5f);

        //Fade transition 2 to 3 
        image[2].enabled = true; //Enable the next image
        animatedPieces[1].SetActive(false); //Set the old active pieces false
        text[1].enabled = false; //Set the old text false
        image[1].CrossFadeAlpha(0, .5f, false); //Set the old image to fade out
        yield return new WaitForSeconds(1f);

        //Cutscene 3
        animatedPieces[2].SetActive(true);
        text[2].enabled = true;
        TE[2].TEffect();
        yield return new WaitForSeconds(6f);

        //Fade transition 3 to 4
        image[3].enabled = true; //Enable the next image
        animatedPieces[2].SetActive(false); //Set the old active pieces false
        text[2].enabled = false; //Set the old text false
        image[2].CrossFadeAlpha(0, .5f, false); //Set the old image to fade out

        //Cutscene 4
        animatedPieces[3].SetActive(true);
        //animatedPieces[5].SetActive(true);
        text[3].enabled = true;
        TE[3].TEffect();
        yield return new WaitForSeconds(3f);
        //lines
        animatedPieces[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(false);
        //animatedPieces[5].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animatedPieces[4].SetActive(true);
        yield return new WaitForSeconds(3f);
        //Set text 4 false, set image 3 false, set image 4 true, set text 5 true //CUTSCENE 4//
        text[3].enabled = false;
        //Then load the next level
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(levelToLoad);
    }

    public void SkipScene()
    {
        if (InputManager.Jump())
        {
            StopCoroutine(ShowCutscenes());
            SceneManager.LoadScene(levelToLoad);
        }
    }


}

