using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExpDB : MonoBehaviour
{
    // Declaring all of the variables and inputs that the user enters
    public InputField emailInput;
    public InputField passwordInput;
    public InputField usernameInput;
    public int value = 1;
    public Text output;

    //Creating the database file and what it will be named
    private string dbName = "URI=file:Users.db";
    void Start()
    {
        CreateDB();
    }

    // Ensures that only one instance of the database exists across scenes
    void Awake()
    {
        if (FindObjectsByType<ExpDB>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    //Creating the database and using the name for the file that we previously gave and creating the table in case that the file had not been made before
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

    //Adding a user into the table with the information that the user enters and chacking to see that the user doesn't already exist
    public void AddUser()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) )
        {
            output.text = "All fields are required.";
            return;
        }

        if(UserExists(username))
        {
            output.text = "User Already Exists.";
            return;
        }

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
            SceneManager.LoadScene("LevelSelection");
            //output.text = "User Registered Successfully!";
        }
    }

    //Checks to see if the information entered matches with an existing account
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

    //Used for the Login buttin to have users change scenes to the Login screen
    public void changeScene()
    {
        SceneManager.LoadScene("Login");
    }


    // updates experiment completion progress
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

    // gets experiment completion progress
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
