using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
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

    private IEnumerator ReloadSceneRoutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        ReloadGame();
    }
}
