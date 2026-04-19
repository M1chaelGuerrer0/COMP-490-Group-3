using UnityEngine;
using UnityEngine.SceneManagement;

// in charge of changing scenes and quitting the game
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // reset time 
        TaskManager.IsInteractionLocked = false; // reset pause
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}