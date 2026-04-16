using UnityEngine;
using UnityEngine.UI;

public class ExperimentButton : MonoBehaviour
{
    [SerializeField] private int experimentIndex;
    private Button button;

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