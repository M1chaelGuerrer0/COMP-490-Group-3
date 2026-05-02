using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles user account storage and experiment progress using a local SQLite database.
/// </summary>
public class ExpDB : MonoBehaviour
{
    public int value = 1;
    public Text output;

    // SQLite file path for the local database
    private string dbName = "URI=file:Users.db";

    // Create the database file and table if they do not already exist
    void Start()
    {
        CreateDB();
    }

    // Ensure only one ExpDB persists across scene loads
    void Awake()
    {
        if (FindObjectsByType<ExpDB>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    // Create the database and ensure the Accounts table exists
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

    // Add a new user record and then load the main menu scene
    public void AddUser(string username, string email, string password)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Accounts (email, username, password, experiment) VALUES (@email, @username, @password, @experiment)";
                command.Parameters.Add(new SqliteParameter("@email", email));
                command.Parameters.Add(new SqliteParameter("@username", username));
                command.Parameters.Add(new SqliteParameter("@password", password));
                command.Parameters.Add(new SqliteParameter("@experiment", value));

                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        UserSession.CurrentUsername = username;
        SceneManager.LoadScene("MainMenu");
    }

    // Check whether a username already exists in the Accounts table
    public bool UserExists(string username)
    {
        bool exists = false;
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT 1 FROM Accounts WHERE username = @username LIMIT 1";
                command.Parameters.Add(new SqliteParameter("@username", username)); 
                
                using(IDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        exists = true;
                    }
                }
            }
            connection.Close();
        }
        return exists;

    }

    // Load the login scene
    public void changeScene()
    {
        SceneManager.LoadScene("Login");
    }

    // Update the saved experiment progress for the current user
    public void UpdateProgress(int newExperiment)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Accounts SET experiment = @experiment WHERE username = @username";

                command.Parameters.Add(new SqliteParameter("@experiment", newExperiment));
                command.Parameters.Add(new SqliteParameter("@username", UserSession.CurrentUsername));

                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        Debug.Log("Progress updated to: " + newExperiment);
    }

    // Retrieve the saved experiment progress for the current user
    public int GetProgress()
    {
        int experiment = 1;

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT experiment FROM Accounts WHERE username = @username";
                command.Parameters.Add(new SqliteParameter("@username", UserSession.CurrentUsername));

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        experiment = reader.GetInt32(0);
                    }
                }
            }

            connection.Close();
        }

        return experiment;
    }
}
