using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene changes and quitting the application.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load a new scene and restore normal game time and interaction state.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // reset time
        TaskManager.IsInteractionLocked = false; // reset pause
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quit the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}