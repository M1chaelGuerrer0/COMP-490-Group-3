using TMPro;
using UnityEngine;

/// <summary>
/// Updates the main menu play button text based on the current user type.
/// </summary>
public class PlayButtonLabel : MonoBehaviour
{
    private TMP_Text label;

    void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Set the button label for guests or signed-in users.
    /// </summary>
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