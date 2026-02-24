using UnityEngine;

// This class represents an ingredient in the game, 
//      which is associated with a specific task that 
//      needs to be completed when the ingredient is interacted with.
public class Ingredient : MonoBehaviour
{
    public string ingredientID;
    public Task taskToComplete;
}