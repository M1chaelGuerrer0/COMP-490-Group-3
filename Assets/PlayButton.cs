using TMPro;
using UnityEngine;

public class PlayButtonLabel : MonoBehaviour
{
    private TMP_Text label;

    void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        if (UserSession.CurrentUsername == "Guest")
        {
            label.text = "Play as Guest";
            label.fontSize = 30; // smaller
        }
        else
        {
            label.text = "Play";
            label.fontSize = 70; // bigger
        }
    }
}