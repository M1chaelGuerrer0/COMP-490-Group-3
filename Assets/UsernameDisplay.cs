using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current logged-in username in a TMP text component.
/// </summary>
public class UsernameDisplay : MonoBehaviour
{
    private TMP_Text text;

    // Cache the TMP_Text component on awake
    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Set the displayed username when the scene starts
    void Start()
    {
        text.text = "User: " + UserSession.CurrentUsername;
    }
}