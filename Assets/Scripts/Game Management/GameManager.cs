using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sierra;

public delegate void HitStopEventHandler();

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool HitStopActive;

    private IEnumerator currentHitStopRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    /// <summary>
    /// Freezes characters and projectiles in place for a specified duration.
    /// </summary>
    /// <param name="duration"></param>
    public void HitStopFor(int frames)
    {
        Debug.Log("Hitstop for " + frames);

        if (currentHitStopRoutine != null) StopCoroutine(currentHitStopRoutine);
        currentHitStopRoutine = HitStopRoutine(Utility.FramesToSeconds(frames));
        StartCoroutine(currentHitStopRoutine);
    }
    /// <summary>
    /// Reloads the active scene
    /// </summary>
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Reloads the scene after a specified duration
    /// </summary>
    /// <param name="timer">delay until reload in seconds</param>
    public void ReloadGame(float timer)
    {
        StartCoroutine(ReloadSceneRoutine(timer));
    }
    public IEnumerator UntillHitStopInactive()
    {
        while (HitStopActive) yield return null;
    }

    private IEnumerator ReloadSceneRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        ReloadGame();
    }
    private IEnumerator HitStopRoutine(float timer)
    {
        HitStopActive = true;
        yield return new WaitForSeconds(timer);

        HitStopActive = false;
    }
}
