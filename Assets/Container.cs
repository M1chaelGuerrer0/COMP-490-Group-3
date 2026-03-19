using UnityEngine;

// This script should be attached to the container GameObject (e.g., flask, cup).
public class Container : MonoBehaviour
{
    [System.Serializable]
    public class IngredientSpritePair
    {
        public string ingredientID; // What this container accepts
        public Sprite newSprite; // What this container changes to
        public string transformToID; // If set → transforms the dragged object (tool behavior)
        public Task taskToComplete; // Optional task to complete when accepted
    }
    
    [Header("Accepted Ingredients")]
    [SerializeField] private IngredientSpritePair[] acceptedIngredients;

    [Header("Tool Reset Sprite (Optional)")]
    [SerializeField] private Sprite defaultToolSprite;

    private SpriteRenderer spriteRenderer;
    private TaskManager taskManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    // This method is called when an ingredient is added to the container.
    public void TryAccept(GameObject obj)
    {
        Ingredient ingredient = obj.GetComponent<Ingredient>();
        if (ingredient == null) return;

        foreach (IngredientSpritePair pair in acceptedIngredients)
        {
            // Accept either exact match OR base tool (for flexibility like SpoonSoap → Sink)
            if (ingredient.ingredientID == pair.ingredientID ||
                ingredient.baseIngredientID == pair.ingredientID)
            {
                Debug.Log("Accepted: " + pair.ingredientID);

                // CASE 1: TRANSFORM TOOL (e.g., Spoon → SoapLoaded)
                if (!string.IsNullOrEmpty(pair.transformToID))
                {
                    ingredient.SetIngredient(pair.transformToID, null);

                    // Change TOOL sprite
                    SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
                    if (objRenderer != null && pair.newSprite != null)
                    {
                        objRenderer.sprite = pair.newSprite;
                    }

                    Debug.Log("Tool now holds: " + pair.transformToID);
                    return;
                }

                // CASE 2: NORMAL USE (e.g., SpoonSoap → Flask, Yeast → Cup)

                // Complete task
                if (pair.taskToComplete != null)
                {
                    pair.taskToComplete.CompleteTask();
                }

                // Change CONTAINER sprite
                if (pair.newSprite != null)
                    spriteRenderer.sprite = pair.newSprite;

                // Reset tool back to original state (Spoon, Syringe, etc.)
                ingredient.ResetIngredient();

                // Reset TOOL sprite
                SpriteRenderer toolRenderer = obj.GetComponent<SpriteRenderer>();
                if (toolRenderer != null && defaultToolSprite != null)
                {
                    toolRenderer.sprite = defaultToolSprite;
                }

                Debug.Log("Tool reset to: " + ingredient.baseIngredientID);
                return;
            }
        }

        // Wrong ingredient
        Debug.Log("Wrong ingredient added.");
        if (taskManager != null)
            taskManager.AddPenalty();
    }
}