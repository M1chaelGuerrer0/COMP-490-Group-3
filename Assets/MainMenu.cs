using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("Level 1 clicked");
        SceneManager.LoadScene("ElephantToothpaste");
    }
}