using UnityEngine;

// Represents anything usable in the experiment
public class Ingredient : MonoBehaviour
{
    public string ingredientID;

    // to remember original identity (Spoon, Syringe, etc.)
    public string baseIngredientID;

    void Awake()
    {
        // If not set, default to starting ID
        if (string.IsNullOrEmpty(baseIngredientID))
        {
            baseIngredientID = ingredientID;
        }
    }

    // Change what this object currently represents
    public void SetIngredient(string newID, Task newTask)
    {
        ingredientID = newID;
    }

    // Reset back to original state (e.g., Spoon, Syringe, etc.)
    public void ResetIngredient()
    {
        ingredientID = baseIngredientID;
    }
}