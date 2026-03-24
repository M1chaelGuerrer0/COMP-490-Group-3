using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("Level 1 clicked");
        SceneManager.LoadScene("ElephantToothpaste");
    }
}