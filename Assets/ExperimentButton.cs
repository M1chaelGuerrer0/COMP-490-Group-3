using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a level-selection button for a specific experiment.
/// Locks the button if the current user is not logged in or has not reached that experiment.
/// </summary>
public class ExperimentButton : MonoBehaviour
{
    [SerializeField] private int experimentIndex;
    private Button button;

    // Initialize the button and disable it when the experiment is not yet unlocked
    void Start()
    {
        
        button = GetComponent<Button>();

        // --- Not logged in ---
        if (string.IsNullOrEmpty(UserSession.CurrentUsername))
        {
            Debug.LogWarning("No user logged in → locking button");
            button.interactable = false;
            button.image.color = Color.red;
            return;
        }

        // --- Get progress ---
        ExpDB db = FindFirstObjectByType<ExpDB>();

        if (db != null)
        {
            int progress = db.GetProgress();

            Debug.Log("Progress: " + progress + " | Button index: " + experimentIndex);

            if (progress >= experimentIndex)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
                button.image.color = Color.red;
            }
        }
    }
}