using UnityEngine;

/// <summary>
/// Represents an ingredient or tool used in the experiment.
/// Tracks the current identity and whether it should remain after a successful interaction.
/// </summary>
public class Ingredient : MonoBehaviour
{
    public string ingredientID;

    // Original identity to restore after a tool has been transformed
    public string baseIngredientID;

    [SerializeField] private bool stayAfterUse = false;
    public bool StayAfterUse => stayAfterUse;

    // If true, the object stays in its new position after being accepted by a container.
    // If false, it will return to its original position.

    void Awake()
    {
        // Default baseIngredientID to the starting ingredient if not already assigned
        if (string.IsNullOrEmpty(baseIngredientID))
        {
            baseIngredientID = ingredientID;
        }
    }

    /// <summary>
    /// Update the ingredient ID to a new state.
    /// </summary>
    public void SetIngredient(string newID, Task newTask)
    {
        ingredientID = newID;
    }

    /// <summary>
    /// Restore the ingredient back to its original base identity.
    /// </summary>
    public void ResetIngredient()
    {
        ingredientID = baseIngredientID;
    }
}