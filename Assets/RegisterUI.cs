using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the registration UI and creates a new user account.
/// </summary>
public class RegisterUI : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField usernameInput;
    public TextMeshProUGUI output;

    private ExpDB db;

    void Start()
    {
        db = FindFirstObjectByType<ExpDB>();
    }

    /// <summary>
    /// Validate registration fields and add a new user if the username is available.
    /// </summary>
    public void Register()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            output.text = "All fields are required.";
            return;
        }

        if (db.UserExists(username))
        {
            output.text = "User Already Exists.";
            return;
        }

        db.AddUser(username, email, password);
    }
}