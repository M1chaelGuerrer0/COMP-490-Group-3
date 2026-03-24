using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("BUTTON WORKED");
        SceneManager.LoadScene("ElephantToothpaste");
    }
}