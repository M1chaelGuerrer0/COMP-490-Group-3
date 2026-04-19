using TMPro;
using UnityEngine;

// changes the text of the play button in main menu based on if user is a "Guest"
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