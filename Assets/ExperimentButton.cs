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
    void Start() {
        button = GetComponent<Button>();

        int progress;

        if (UserSession.CurrentUsername == "Guest") {
            progress = SessionProgress.CurrentExperiment;
        }
        else {
            ExpDB db = FindFirstObjectByType<ExpDB>();

            if (db != null) {
                progress = db.GetProgress();
            }
            else {
                progress = 1; // fallback
            }
        }

        if (progress >= experimentIndex) {
            button.interactable = true;
        }
        else {
            button.interactable = false;
            button.image.color = Color.red;
        }
    }
}