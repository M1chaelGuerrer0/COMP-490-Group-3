using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterUI : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField usernameInput;
    public Text output;

    private ExpDB db;

    void Start()
    {
        db = FindFirstObjectByType<ExpDB>();
    }

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