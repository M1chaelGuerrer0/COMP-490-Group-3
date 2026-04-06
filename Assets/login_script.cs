using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using static UnityEditor.Rendering.CameraUI;
using System;
using UnityEngine.SceneManagement;

public class login_script : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text result;

    private string dbName = "URI=file:Users.db";
    void Start()
    {
        CreateDB();
    }

    //Same processs here just put here to make sure that user has a database file
    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Accounts (UserID INTEGER PRIMARY KEY AUTOINCREMENT, email VARCHAR(30) NOT NULL, username VARCHAR(20) NOT NULL, password VARCHAR(20) NOT NULL, level INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    // will chack to see if there is a user and that they entered the information correctly
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
                    SceneManager.LoadScene("LevelSelection");
                }
                else
                {
                    result.text = "Incorrect username and/or password";
                }
                
            }
            connection.Close();
            
        }

    }

    //Used for the Register button to take users to the registration screen
    public void changeScene()
    {
        SceneManager.LoadScene("Register");
    }

    
}
