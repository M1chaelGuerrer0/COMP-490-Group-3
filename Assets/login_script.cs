using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles login validation and scene switching for the login UI.
/// </summary>
public class login_script : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public TextMeshProUGUI result;

    private string dbName = "URI=file:Users.db";

    void Start()
    {
        CreateDB();
    }

    // Ensure the local SQLite database and Accounts table exist
    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Accounts (UserID INTEGER PRIMARY KEY AUTOINCREMENT, email VARCHAR(30) NOT NULL, username VARCHAR(20) NOT NULL, password VARCHAR(20) NOT NULL, experiment INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    // Validate the entered username and password against the Accounts table
    public void ValidateUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        if (string.IsNullOrEmpty(username)  || string.IsNullOrEmpty(password))
        {
            result.text = "All fields are required.";
            return;
        }

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Accounts WHERE username = @username AND password = @password";
                command.Parameters.Add(new SqliteParameter("@username", username));
                command.Parameters.Add(new SqliteParameter("@password", password));
                int userCount = Convert.ToInt32(command.ExecuteScalar());
                if (userCount > 0)
                {
                    //result.text = "Login Successful!";
                    UserSession.CurrentUsername = username;
                    Debug.Log(UserSession.CurrentUsername);
                    // goto level selection
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    result.text = "Incorrect username and/or password";
                }
                
            }
            connection.Close();
            
        }

    }

    // Load the registration scene when the user clicks Register
    public void changeScene()
    {
        SceneManager.LoadScene("Register");
    }
}

